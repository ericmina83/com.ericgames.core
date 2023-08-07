using UnityEngine;
using EricGames.Runtime.StateMachine;

namespace EricGames.Runtime.Characters
{
    public partial class Character
    {
        protected bool doubleJump = true;

        public delegate void JumpDelegate();

        public event JumpDelegate OnJump;

        private void InitStateJump()
        {
            var jumpState = movementStateMachine.GetSubState(MovementState.JUMP);

            jumpState.RegisterStateDelegate(StateDelegateType.START, JumpStateStart);
            jumpState.RegisterStateDelegate(StateDelegateType.UPDATE, JumpStateUpdate);

            jumpState.RegisterTransition(MovementState.JUMP, 0.25f,
                () => triggerHandler.GetTriggerValue(TriggerType.JUMP) && doubleJump);
            jumpState.RegisterTransition(MovementState.FALL, 0.25f,
                () => landingState == LandingState.FALLING);
            jumpState.RegisterTransition(MovementState.MOVE, 0.25f,
                () => landingState == LandingState.GROUNDED);
        }

        #region Trigger Function

        virtual public void Jump()
        {
            triggerHandler.SetTrigger(TriggerType.JUMP, 0.4f);
        }

        #endregion

        #region State Delegate

        private void JumpStateStart()
        {
            if (landingState == LandingState.GROUNDED)
            {
                doubleJump = true;
            }
            else
            {
                doubleJump = false;
            }

            landingState = LandingState.JUMPING;
            HandleApplyJumpForce(jumpForce);

            triggerHandler.ResetTrigger(TriggerType.JUMP);
            animatorTriggerHandler.SetTrigger(jumpParameterHash, 0.4f);

            OnJump?.Invoke();
        }

        private void JumpStateUpdate()
        {
            ApplyRotation();

            HandleJump(moveInput, lookInput);
        }

        protected abstract void HandleApplyJumpForce(float jumpForce);

        protected abstract void HandleJump(Vector2 moveInput, Vector3 lookInput);

        #endregion

    }
}