using System;
using System.Collections.Generic;
using EricGames.Core.Components;

namespace EricGames.Core.StateMachine
{
    public enum StateDelegateType
    {
        START,
        UPDATE,
        END
    }

    public class SubState<StateType, TriggerType>
        where StateType : Enum
        where TriggerType : Enum
    {
        public delegate void StateDelegate();

        internal StateMachine<StateType, TriggerType> stateMachine;
        public Dictionary<StateDelegateType, StateDelegate> stateDelegates = new Dictionary<StateDelegateType, StateDelegate>();
        public List<Transition<StateType, TriggerType>> transitions = new List<Transition<StateType, TriggerType>>();

        public SubState(StateMachine<StateType, TriggerType> stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public void InvokeTargetDelegate(StateDelegateType stateDelegateType)
        {
            if (stateDelegates.TryGetValue(stateDelegateType, out var stateDelegate))
            {
                stateDelegate.Invoke();
            }
        }

        public void RegisterTransition(
            StateType sourceState,
            StateType targetState,
            float exitTime,
            TriggerType[] triggerTypes,
            ConditionDelegate conditionDelegate)
        {
            transitions
                .Add(new Transition<StateType, TriggerType>(
                    targetState,
                    conditionDelegate,
                    exitTime,
                    triggerTypes,
                    stateMachine));
        }

        public virtual void ReigsterStateDelegate(StateType state, StateDelegateType delegateType, StateDelegate stateDelegate)
        {
            stateDelegates.Add(delegateType, stateDelegate);
        }
    }
}