using UnityEngine;
using EricGames.Core.StateMachine;

namespace EricGames.Core.Characters
{
    public partial class Character
    {
        protected bool doubleJump = true;
        private bool applyJumpForce = false;

        private void InitStateJump()
        {
            var jumpState = movementStateMachine.GetSubState(MovementState.JUMP);

            jumpState.ReigsterStateDelegate(StateDelegateType.START, JumpStateStart);
            jumpState.ReigsterStateDelegate(StateDelegateType.UPDATE, JumpStateUpdate);

            jumpState.RegisterTransition(MovementState.JUMP, 0.0f,
                () => landingState == LandingState.GROUNDED ? true : doubleJump
                    && triggerHandler.GetTriggerValue(TriggerType.JUMP));
            jumpState.RegisterTransition(MovementState.FALL, 0.0f,
                () => landingState != LandingState.GROUNDED && speedY < 0);
            jumpState.RegisterTransition(MovementState.MOVE, 0.0f,
                () => landingState == LandingState.GROUNDED);
        }

        #region Trigger Function

        virtual public void Jump()
        {
            triggerHandler.SetTrigger(TriggerType.JUMP, 0.4f);
            applyJumpForce = true;
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
                doubleJump = !doubleJump;
            }

            if (applyJumpForce)
            {
                applyJumpForce = false;
                HandleApplyJumpForce(jumpForce);
            }

            triggerHandler.ResetTrigger(TriggerType.JUMP);
            animatorTriggerHandler.SetTrigger(jumpParameterHash, 0.1f);
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