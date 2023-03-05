using System;
using System.Collections.Generic;
using EricGames.Core.Components;

namespace EricGames.Core.StateMachine
{
    public class SubStateMachine<ParentStateType, StateType, TriggerType>
    : SubState<ParentStateType, TriggerType>
    where ParentStateType : Enum
    where StateType : Enum
    where TriggerType : Enum
    {
        internal StateMachine<StateType, TriggerType> subStateMachine;

        public SubStateMachine(SubState<ParentStateType, TriggerType> state, StateType defaultStateType) : base(state.stateMachine)
        {
            transitions = state.transitions;
            stateDelegates = state.stateDelegates;

            subStateMachine = new StateMachine<StateType, TriggerType>(defaultStateType);
        }

        public override void ReigsterStateDelegate(ParentStateType state, StateDelegateType delegateType, StateDelegate stateDelegate)
        {
            if (delegateType == StateDelegateType.UPDATE)
            {
                throw new Exception("SubStateMachine can't Register DelegateType - UPDATE");
            }

            base.ReigsterStateDelegate(state, delegateType, stateDelegate);
        }
    }
}