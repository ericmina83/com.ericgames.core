using System;
using System.Collections.Generic;
using EricGames.Core.Components;

namespace EricGames.Core.StateMachine
{
    public delegate bool ConditionDelegate();

    public class Transition<StateType, TriggerType>
    where StateType : Enum
    where TriggerType : Enum
    {

        public StateType targetState;
        internal ConditionDelegate conditionDelegate;
        private TriggerType[] triggerTypes;
        private float exitTime;
        private StateMachine<StateType, TriggerType> stateMachine;
        public bool exitTransition = false;

        public Transition(
            StateType targetState,
            ConditionDelegate conditionDelegate,
            float exitTime,
            TriggerType[] triggerTypes,
            StateMachine<StateType, TriggerType> stateMachine)
        {
            this.targetState = targetState;
            this.conditionDelegate = conditionDelegate;
            this.triggerTypes = triggerTypes;
            this.stateMachine = stateMachine;
        }

        public Transition(
            ConditionDelegate conditionDelegate,
            float exitTime,
            TriggerType[] triggerTypes,
            StateMachine<StateType, TriggerType> stateMachine)
        {
            this.exitTransition = true;
            this.conditionDelegate = conditionDelegate;
            this.triggerTypes = triggerTypes;
            this.stateMachine = stateMachine;
        }

        public bool CheckCondition(float deltaTime)
        {
            var timeout = exitTime <= 0;
            exitTime -= deltaTime;

            if (!timeout)
                return false;

            var result = true;

            if (conditionDelegate != null)
                result &= conditionDelegate.Invoke();

            if (triggerTypes != null)
            {
                foreach (var triggerType in triggerTypes)
                {
                    result &= stateMachine.triggerHandler.GetTriggerValue(triggerType);
                }

                if (result)
                {
                    foreach (var triggerType in triggerTypes)
                    {
                        stateMachine.triggerHandler.ResetTrigger(triggerType);
                    }
                }
            }

            return result;
        }
    }
}