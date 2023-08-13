using UnityEngine;

namespace EricGames.Runtime.Characters
{
    public partial class Character
    {
        private void InitStateMove()
        {
            var moveState = movementStateMachine.GetSubState(MovementState.MOVE);

            moveState.StateUpdateEvent += MoveStateUpdate;

            moveState.RegisterTransition(MovementState.JUMP, 0.0f,
                () => landingState == LandingState.GROUNDED && triggerHandler.GetTriggerValue(TriggerType.JUMP));
            moveState.RegisterTransition(MovementState.FALL, 0.0f,
                () => landingState == LandingState.FALLING);
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