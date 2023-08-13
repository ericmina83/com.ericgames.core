using System;
using System.Collections.Generic;

namespace EricGames.Runtime.StateMachine
{
    public class State<StateType>
        where StateType : Enum
    {
        private event Action stateStartEvent;
        public event Action StateStartEvent
        {
            remove => stateStartEvent -= value;
            add => stateStartEvent += value;
        }

        private event Action stateUpdateEvent;
        public event Action StateUpdateEvent
        {
            remove => stateUpdateEvent -= value;
            add => stateUpdateEvent += value;
        }

        private event Action stateEndEvent;
        public event Action StateEndEvent
        {
            remove => stateEndEvent -= value;
            add => stateEndEvent += value;
        }

        public readonly List<Transition<StateType>> transitions = new();

        public void InvokeStateStartEvent() => stateStartEvent?.Invoke();
        public void InvokeStateUpdateEvent() => stateUpdateEvent?.Invoke();
        public void InvokeStateEndEvent() => stateEndEvent?.Invoke();

        virtual public void RegisterTransition(
            StateType targetState,
            float? exitTime,
            ConditionDelegate conditionDelegate)
        {
            transitions.Add(new Transition<StateType>(targetState, exitTime, conditionDelegate));
        }
    }
}