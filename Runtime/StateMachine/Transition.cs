using System;
using System.Collections.Generic;
using EricGames.Core.Components;

namespace EricGames.Core.StateMachine
{
    public delegate bool ConditionDelegate();

    public class Transition<StateType>
    where StateType : Enum
    {

        public StateType targetState;
        internal ConditionDelegate conditionDelegate;
        private float exitTime;
        public bool exitTransition = false;

        public Transition(
            StateType targetState,
            float exitTime,
            ConditionDelegate conditionDelegate)
        {
            this.targetState = targetState;
            this.conditionDelegate = conditionDelegate;
        }

        public Transition(float exitTime, ConditionDelegate conditionDelegate)
        {
            this.exitTransition = true;
            this.conditionDelegate = conditionDelegate;
        }

        public bool CheckCondition(float deltaTime)
        {
            var timeout = exitTime <= 0;
            exitTime -= deltaTime;

            if (!timeout)
                return false;

            if (conditionDelegate != null)
                return conditionDelegate.Invoke();

            return true;
        }
    }
}