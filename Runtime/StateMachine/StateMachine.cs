using System;
using System.Collections.Generic;
using EricGames.Core.Components;

namespace EricGames.Core.StateMachine
{
    public class StateMachine<StateType, TriggerType>
        where StateType : Enum
        where TriggerType : Enum
    {
        private List<Transition<StateType, TriggerType>> startTransitions = new List<Transition<StateType, TriggerType>>();
        internal TriggerHandler<TriggerType> triggerHandler = new TriggerHandler<TriggerType>();

        private StateType defaultState;
        private StateType currStateType;
        private StateType nextStateType;

        public StateType CurrState => currStateType;
        public StateType NextState => nextStateType;

        private bool stateChanged = false;
        internal bool stateMachineStop = false;
        private bool exit = false;


        private Dictionary<StateType, SubState<StateType, TriggerType>> subStateMaps
            = new Dictionary<StateType, SubState<StateType, TriggerType>>();

        public StateMachine(StateType defaultStateType)
        {
            defaultState = defaultStateType;
            stateMachineStop = true;

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

            if (stateMachineStop) // Initialize
            {
                nextStateType = defaultState;

                foreach (var transition in startTransitions)
                {
                    if (transition.CheckCondition(deltaTime))
                    {
                        nextStateType = transition.targetState;
                        break;
                    }
                }

                subStateMaps[nextStateType].InvokeTargetDelegate(StateDelegateType.START);
                currStateType = nextStateType;
                stateChanged = stateMachineStop = false;
            }
            else
            {
                var currState = subStateMaps[currStateType];
                if (!currState.Stay)
                {
                    foreach (var transition in currState.transitions)
                    {
                        if (transition.CheckCondition(deltaTime))
                        {
                            if (transition.exitTransition)
                            {
                                exit = true;
                            }
                            else
                            {
                                nextStateType = transition.targetState;
                            }
                            stateChanged = true;
                            break;
                        }
                    }
                }

                if (stateChanged)
                {
                    currState.InvokeTargetDelegate(StateDelegateType.END);

                    if (exit)
                    {
                        stateMachineStop = true;
                    }
                    else
                    {
                        subStateMaps[nextStateType].InvokeTargetDelegate(StateDelegateType.START);

                        currStateType = nextStateType;
                    }

                    stateChanged = false;
                }
            }

            if (!stateMachineStop)
            {
                subStateMaps[currStateType].InvokeTargetDelegate(StateDelegateType.UPDATE);
            }
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