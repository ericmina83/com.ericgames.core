using System;
using System.Collections.Generic;
using EricGames.Runtime.Components;

namespace EricGames.Runtime.StateMachine
{
    public delegate bool ConditionDelegate();

    public delegate bool CallbackDelegate(bool pass);

    public class Transition<StateType>
    where StateType : Enum
    {

        public StateType targetState;
        internal ConditionDelegate conditionDelegate;
        private readonly float? exitTime = null;

        public Transition(
            StateType targetState,
            float? exitTime,
            ConditionDelegate conditionDelegate)
        {
            this.exitTime = exitTime;
            this.targetState = targetState;
            this.conditionDelegate = conditionDelegate;
        }

        public bool CheckCondition(float currentStateTime = 0.0f)
        {
            var timeout = (exitTime != null) ? currentStateTime > exitTime : true;

            if (!timeout)
                return false;

            if (conditionDelegate != null)
                return conditionDelegate.Invoke();

            return true;
        }
    }
}