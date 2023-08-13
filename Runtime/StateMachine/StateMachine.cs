using System;
using System.Collections.Generic;

namespace EricGames.Runtime.StateMachine
{
    public class StateMachine<StateType>
        where StateType : Enum
    {
        private StateType currStateType;
        public StateType CurrState => currStateType;

        private StateType nextStateType;
        public StateType NextState => nextStateType;

        private readonly Dictionary<StateType, State<StateType>> subStateMaps = new();

        private readonly List<Transition<StateType>> startTransitions = new();

        private readonly StateType defaultStateType;

        private bool initMachine = false;
        private bool stateChanged = false;
        private float currentStateTime = 0;

        public StateMachine(StateType defaultStateType)
        {
            this.defaultStateType = defaultStateType;

            foreach (StateType state in Enum.GetValues(typeof(StateType)))
            {
                subStateMaps.Add(state, new());
            }

            Reset();
        }

        public State<StateType> GetSubState(StateType stateType) => subStateMaps[stateType];

        public void Reset()
        {
            stateChanged = true;
            initMachine = true;
            nextStateType = defaultStateType;
        }

        virtual public void RegisterStartTransition(StateType targetState, float? exitTime, ConditionDelegate conditionDelegate)
           => startTransitions.Add(new Transition<StateType>(targetState, exitTime, conditionDelegate));

        public void Tick(float deltaTime)
        {
            if (initMachine)
            {
                foreach (var transition in startTransitions)
                {
                    if (transition.CheckCondition())
                    {
                        nextStateType = transition.targetState;
                        break;
                    }
                }
                initMachine = false;
            }

            if (stateChanged) // Initialize
            {
                subStateMaps[nextStateType].InvokeStateStartEvent();
                currStateType = nextStateType;
                currentStateTime = 0.0f;
                stateChanged = false;
            }

            var currState = subStateMaps[currStateType];

            currState.InvokeStateUpdateEvent();

            foreach (var transition in currState.transitions)
            {
                if (transition.CheckCondition(currentStateTime))
                {
                    nextStateType = transition.targetState;
                    stateChanged = true;
                    break;
                }
            }

            if (stateChanged)
            {
                currState.InvokeStateEndEvent();
            }

            currentStateTime += deltaTime;
        }
    }
}