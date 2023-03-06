using System;
using UnityEngine;

namespace EricGames.Core.StateMachine
{
    public class SubStateMachine<ParentStateType, StateType, TriggerType>
    : SubState<ParentStateType, TriggerType>
    where ParentStateType : Enum
    where StateType : Enum
    where TriggerType : Enum
    {
        internal StateMachine<StateType, TriggerType> subStateMachine;
        override internal bool Stay => subStateMachine.stateMachineStop;

        public SubStateMachine(SubState<ParentStateType, TriggerType> state, StateType defaultStateType) : base(state.stateMachine)
        {
            transitions = state.transitions;
            stateDelegates = state.stateDelegates;

            subStateMachine = new StateMachine<StateType, TriggerType>(defaultStateType);

            stateDelegates[StateDelegateType.UPDATE] = () => subStateMachine.Tick(Time.deltaTime);
        }

        public override void ReigsterStateDelegate(StateDelegateType delegateType, StateDelegate stateDelegate)
        {
            if (delegateType == StateDelegateType.UPDATE)
            {
                throw new Exception("SubStateMachine can't Register DelegateType - UPDATE");
            }

            base.ReigsterStateDelegate(delegateType, stateDelegate);
        }

        public override void RegisterExitTransition(float exitTime, TriggerType[] triggerTypes, ConditionDelegate conditionDelegate)
        {
            base.RegisterExitTransition(exitTime, triggerTypes, conditionDelegate);
        }
    }
}