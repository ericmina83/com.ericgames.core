using System;
using System.Collections.Generic;

namespace EricGames.Runtime.StateMachine
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

        public readonly Dictionary<StateDelegateType, List<StateDelegate>> stateDelegates = new();
        public readonly List<Transition<StateType>> transitions = new();

        public void InvokeTargetDelegate(StateDelegateType stateDelegateType)
        {
            if (stateDelegates.TryGetValue(stateDelegateType, out var delegates))
            {
                foreach (var stateDelegate in delegates)
                {
                    stateDelegate.Invoke();
                }
            }
        }

        virtual public void RegisterTransition(
            StateType targetState,
            float exitTime,
            ConditionDelegate conditionDelegate)
        {
            transitions.Add(new Transition<StateType>(targetState, exitTime, conditionDelegate));
        }

        public virtual void RegisterStateDelegate(
            StateDelegateType delegateType, StateDelegate stateDelegate)
        {
            if (!stateDelegates.TryGetValue(delegateType, out var delegates))
            {
                delegates = new();
                stateDelegates.Add(delegateType, delegates);
            }
            delegates.Add(stateDelegate);
        }
    }
}