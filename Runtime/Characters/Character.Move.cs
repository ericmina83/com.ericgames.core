using UnityEngine;
using EricGames.Core.StateMachine;

namespace EricGames.Core.Characters
{
    public partial class Character
    {
        private void InitStateMove()
        {
            stateMachine.ReigsterStateDelegate(State.MOVE, StateDelegateType.UPDATE, MoveStateUpdate);

            stateMachine.RegisterTransition(State.MOVE, State.DODGE, 0.2f,
                new TriggerType[] { TriggerType.DODGE },
                null);
            stateMachine.RegisterTransition(State.MOVE, State.ATTACK, 0.2f,
                new TriggerType[] { TriggerType.ATTACK },
                null);
            stateMachine.RegisterTransition(State.MOVE, State.JUMP, 0.4f,
                new TriggerType[] { TriggerType.JUMP },
                null);
            stateMachine.RegisterTransition(State.MOVE, State.FALL, 0.0f,
                null,
                () => landingState == LandingState.GROUNDED && 0 < speedY);
        }

        #region Trigger Function

        public void Move(Vector2 input)
        {
            targMoveInput = input;
        }

        #endregion

        #region State Delegate

        private void MoveStateUpdate()
        {
            CheckRotation(targMoveInput.x);

            targMoveInput = Vector2.zero;
        }

        #endregion
    }
}