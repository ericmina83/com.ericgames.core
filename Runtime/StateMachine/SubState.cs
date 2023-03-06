using System;
using System.Collections.Generic;

namespace EricGames.Core.StateMachine
{
    public enum StateDelegateType
    {
        START,
        UPDATE,
        END
    }

    public class State<StateType>
        where StateType : Enum
    {
        public delegate void StateDelegate();

        public Dictionary<StateDelegateType, StateDelegate> stateDelegates = new Dictionary<StateDelegateType, StateDelegate>();
        public List<Transition<StateType>> transitions = new List<Transition<StateType>>();

        public void InvokeTargetDelegate(StateDelegateType stateDelegateType)
        {
            if (stateDelegates.TryGetValue(stateDelegateType, out var stateDelegate))
            {
                stateDelegate.Invoke();
            }
        }

        virtual public void RegisterTransition(
            StateType targetState,
            float exitTime,
            ConditionDelegate conditionDelegate)
        {
            transitions.Add(new Transition<StateType>(targetState, exitTime, conditionDelegate));
        }

        virtual public void RegisterExitTransition(
            float exitTime,
            ConditionDelegate conditionDelegate)
        {
            transitions.Add(new Transition<StateType>(exitTime, conditionDelegate));
        }

        public virtual void ReigsterStateDelegate(
            StateDelegateType delegateType, StateDelegate stateDelegate)
        {
            stateDelegates.Add(delegateType, stateDelegate);
        }
    }
}