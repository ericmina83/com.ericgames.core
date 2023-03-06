using System;
using System.Collections.Generic;
using EricGames.Core.Components;

namespace EricGames.Core.StateMachine
{
    public class StateMachine<StateType>
        where StateType : Enum
    {
        private List<Transition<StateType>> startTransitions = new List<Transition<StateType>>();

        private StateType defaultStateType;
        private StateType currStateType;
        private StateType nextStateType;

        public StateType CurrState => currStateType;
        public StateType NextState => nextStateType;

        internal bool stateMachineStop = false;
        private bool exit = false;

        private Dictionary<StateType, State<StateType>> subStateMaps
            = new Dictionary<StateType, State<StateType>>();

        public StateMachine(StateType defaultStateType)
        {
            this.defaultStateType = defaultStateType;

            foreach (StateType state in Enum.GetValues(typeof(StateType)))
            {
                subStateMaps.Add(state, new());
            }

            Reset();
        }

        public State<StateType> GetSubState(StateType stateType)
        {
            return subStateMaps[stateType];
        }

        public void Reset()
        {
            stateMachineStop = true;
        }

        public void Tick(float deltaTime)
        {
            if (stateMachineStop) // Initialize
            {
                nextStateType = defaultStateType;

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
                stateMachineStop = false;
            }
            else
            {
                var currState = subStateMaps[currStateType];
                var stateChanged = false;

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

                if (stateChanged)
                {
                    currState.InvokeTargetDelegate(StateDelegateType.END);

                    if (exit)
                    {
                        stateMachineStop = true;
                        exit = false;
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

        public void RegisterStartTransition(
            StateType targetState,
            ConditionDelegate conditionDelegate)
        {
            startTransitions.Add(new Transition<StateType>(targetState, 0.0f, conditionDelegate));
        }
    }
}