using System;
using System.Collections.Generic;
using EricGames.Core.Components;

namespace EricGames.Core.StateMachine
{
    public class StateMachine<StateType, TriggerType>
        where StateType : Enum
        where TriggerType : Enum
    {
        internal TriggerHandler<TriggerType> triggerHandler = new TriggerHandler<TriggerType>();

        private StateType currState;
        public StateType CurrState => currState;

        private StateType nextState;
        public StateType NextState => nextState;

        private bool stateChanged = false;
        private bool stateMachineStart = false;

        private Dictionary<StateType, SubState<StateType, TriggerType>> subStateMaps
            = new Dictionary<StateType, SubState<StateType, TriggerType>>();

        public StateMachine(StateType defaultStateType)
        {
            this.currState = this.nextState = defaultStateType;
            stateMachineStart = true;

            foreach (StateType state in Enum.GetValues(typeof(StateType)))
            {
                subStateMaps.Add(state, new SubState<StateType, TriggerType>(this));
            }
        }

        public SubState<StateType, TriggerType> GetSubState(StateType stateType)
        {
            return subStateMaps[stateType];
        }

        public void Tick(float deltaTime)
        {
            triggerHandler.Tick(deltaTime);

            if (stateMachineStart)
            {
                subStateMaps[currState].InvokeTargetDelegate(StateDelegateType.START);
                stateChanged = stateMachineStart = false;
            }
            else
            {
                if (!stateChanged)
                {
                    foreach (var transition in subStateMaps[currState].transitions)
                    {
                        if (transition.CheckCondition(deltaTime))
                        {
                            nextState = transition.targetState;
                            stateChanged = true;
                            break;
                        }
                    }
                }

                if (stateChanged)
                {
                    subStateMaps[currState].InvokeTargetDelegate(StateDelegateType.END);

                    currState = nextState;

                    subStateMaps[currState].InvokeTargetDelegate(StateDelegateType.START);

                    stateChanged = false;
                }
            }

            subStateMaps[currState].InvokeTargetDelegate(StateDelegateType.UPDATE);
        }

        public void SetTrigger(TriggerType triggerType, float time)
        {
            triggerHandler.SetTrigger(triggerType, time);
        }

        public StateMachine<ChildrenStateType, TriggerType> ChangeStateToSubStateMachine<ChildrenStateType>(
            StateType whichStateTypeToChange, ChildrenStateType defaultStateType)
            where ChildrenStateType : Enum
        {
            SubStateMachine<StateType, ChildrenStateType, TriggerType> subStateMachine;
            var currentState = subStateMaps[whichStateTypeToChange];

            if (currentState is SubStateMachine<StateType, ChildrenStateType, TriggerType>)
            {
                subStateMachine
                    = currentState as SubStateMachine<StateType, ChildrenStateType, TriggerType>;
            }
            else
            {
                subStateMaps[whichStateTypeToChange]
                    = subStateMachine
                    = new(currentState, defaultStateType);
            }

            return subStateMachine.subStateMachine;
        }
    }
}