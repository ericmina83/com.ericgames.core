using System;
using System.Collections.Generic;

namespace EricGames.Runtime.StateMachine
{
    public class StateMachine<StateType>
        where StateType : Enum
    {
        private readonly StateType defaultStateType;
        private StateType currStateType;
        private StateType nextStateType;

        public StateType CurrState => currStateType;
        public StateType NextState => nextStateType;

        private bool stateChanged = false;

        private readonly Dictionary<StateType, State<StateType>> subStateMaps
            = new();

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

        public State<StateType> GetSubState(StateType stateType)
        {
            return subStateMaps[stateType];
        }

        public void Reset()
        {
            stateChanged = true;
            nextStateType = defaultStateType;
        }

        public void Tick(float deltaTime)
        {
            if (stateChanged) // Initialize
            {
                subStateMaps[nextStateType].InvokeTargetDelegate(StateDelegateType.START);
                currStateType = nextStateType;
                currentStateTime = 0.0f;
                stateChanged = false;
            }

            var currState = subStateMaps[currStateType];

            currState.InvokeTargetDelegate(StateDelegateType.UPDATE);

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
                currState.InvokeTargetDelegate(StateDelegateType.END);
            }

            currentStateTime += deltaTime;
        }
    }
}