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

    public class StateMachine<StateType, TriggerType>
    where StateType : Enum
    where TriggerType : Enum
    {
        public delegate bool ConditionDelegate();

        private TriggerHandler<TriggerType> triggerHandler = new TriggerHandler<TriggerType>();

        private class Transition
        {
            public StateType targetState;
            private ConditionDelegate conditionDelegate;
            private TriggerType[] triggerTypes;
            private TriggerHandler<TriggerType> triggerHandler;
            private float exitTime;

            public Transition(StateType targetState, ConditionDelegate conditionDelegate, float exitTime, TriggerType[] triggerTypes, TriggerHandler<TriggerType> triggerHandler)
            {
                this.targetState = targetState;
                this.conditionDelegate = conditionDelegate;
                this.triggerTypes = triggerTypes;
                this.triggerHandler = triggerHandler;
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
                        result &= triggerHandler.GetTriggerValue(triggerType);
                    }

                    if (result)
                    {
                        foreach (var triggerType in triggerTypes)
                        {
                            triggerHandler.ResetTrigger(triggerType);
                        }
                    }
                }

                return result;
            }
        }

        public StateMachine(StateType startState)
        {
            this.currState = this.nextState = startState;
            stateMachineStart = true;

            foreach (StateType state in Enum.GetValues(typeof(StateType)))
            {
                stateDelegateMaps.Add(state, new StateDelegateMap(state));
            }
        }

        private StateType currState;
        public StateType CurrState => currState;

        private StateType nextState;
        public StateType NextState => nextState;

        private bool stateChanged = false;
        private bool stateMachineStart = false;

        public delegate void StateDelegate();

        private class StateDelegateMap
        {
            public Dictionary<StateDelegateType, StateDelegate> stateDelegates = new Dictionary<StateDelegateType, StateDelegate>();
            public List<Transition> transitions = new List<Transition>();

            private StateType state;

            public StateDelegateMap(StateType state)
            {
                this.state = state;
            }

            public void InvokeTargetDelegate(StateDelegateType stateDelegateType)
            {
                if (stateDelegates.TryGetValue(stateDelegateType, out var stateDelegate))
                {
                    stateDelegate.Invoke();
                }
            }
        }

        private Dictionary<StateType, StateDelegateMap> stateDelegateMaps = new Dictionary<StateType, StateDelegateMap>();

        public void Tick(float deltaTime)
        {
            triggerHandler.Tick(deltaTime);

            if (stateMachineStart)
            {
                stateDelegateMaps[currState].InvokeTargetDelegate(StateDelegateType.START);
                stateChanged = stateMachineStart = false;
            }
            else
            {
                if (!stateChanged)
                {
                    foreach (var transition in stateDelegateMaps[currState].transitions)
                    {
                        if (transition.CheckCondition(deltaTime))
                        {
                            nextState = transition.targetState;
                            stateChanged = true;
                            break;
                        }
                    }
                }

                if (stateChanged)
                {
                    stateDelegateMaps[currState].InvokeTargetDelegate(StateDelegateType.END);

                    currState = nextState;

                    stateDelegateMaps[currState].InvokeTargetDelegate(StateDelegateType.START);

                    stateChanged = false;
                }
            }

            stateDelegateMaps[currState].InvokeTargetDelegate(StateDelegateType.UPDATE);
        }

        public void ReigsterStateDelegate(StateType state, StateDelegateType delegateType, StateDelegate stateDelegate)
        {
            stateDelegateMaps[state].stateDelegates.Add(delegateType, stateDelegate);
        }

        public void RegisterTransition(StateType sourceState, StateType targetState, float exitTime, TriggerType[] triggerTypes, ConditionDelegate conditionDelegate)
        {
            stateDelegateMaps[sourceState].transitions
                .Add(new Transition(targetState, conditionDelegate, exitTime, triggerTypes, triggerHandler));
        }

        public void SetTrigger(TriggerType triggerType, float time)
        {
            triggerHandler.SetTrigger(triggerType, time);
        }
    }
}