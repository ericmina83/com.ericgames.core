using UnityEngine;
using EricGames.Core.StateMachine;

namespace EricGames.Core.Characters
{
    public partial class Character
    {
        private void InitStateMove()
        {
            var moveState = stateMachine.GetSubState(State.MOVE);

            moveState.ReigsterStateDelegate(State.MOVE, StateDelegateType.UPDATE, MoveStateUpdate);

            moveState.RegisterTransition(State.MOVE, State.DODGE, 0.2f,
                new TriggerType[] { TriggerType.DODGE },
                null);
            moveState.RegisterTransition(State.MOVE, State.ATTACK, 0.2f,
                new TriggerType[] { TriggerType.ATTACK },
                null);
            moveState.RegisterTransition(State.MOVE, State.JUMP, 0.4f,
                new TriggerType[] { TriggerType.JUMP },
                null);
            moveState.RegisterTransition(State.MOVE, State.FALL, 0.0f,
                null,
                () => landingState == LandingState.GROUNDED && 0 < speedY);
        }

        #region Trigger Function

        public void Move(Vector2 input)
        {
            moveInput = input;
        }

        #endregion

        #region State Delegate

        private void MoveStateUpdate()
        {
            ApplyRotation();

            if (!animator.applyRootMotion)
            {
                HandleMove(moveInput);
            }
        }

        #endregion

        #region Abstract Function

        public abstract void HandleMove(Vector2 input);

        #endregion
    }
}