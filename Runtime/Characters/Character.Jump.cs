using UnityEngine;
using EricGames.Core.StateMachine;

namespace EricGames.Core.Characters
{
    public partial class Character
    {
        protected bool doubleJump = true;

        private void InitStateJump()
        {
            var jumpState = stateMachine.GetSubState(State.JUMP);

            jumpState.ReigsterStateDelegate(StateDelegateType.START, JumpStateStart);
            jumpState.ReigsterStateDelegate(StateDelegateType.UPDATE, JumpStateUpdate);

            jumpState.RegisterTransition(State.JUMP, 0.0f,
                new TriggerType[] { TriggerType.JUMP },
                () => landingState == LandingState.GROUNDED ? true : doubleJump);
            jumpState.RegisterTransition(State.FALL, 0.0f,
                null,
                () => landingState != LandingState.GROUNDED && speedY < 0);
            jumpState.RegisterTransition(State.MOVE, 0.0f,
                null,
                () => landingState == LandingState.GROUNDED);
            jumpState.RegisterTransition(State.DODGE, 0.0f,
                new TriggerType[] { TriggerType.DODGE },
                null);
            jumpState.RegisterTransition(State.ATTACK, 0.0f,
                new TriggerType[] { TriggerType.ATTACK },
                null);
        }

        #region Trigger Function

        virtual public void Jump()
        {
            stateMachine.SetTrigger(TriggerType.JUMP, 0.4f);
        }

        #endregion

        #region State Delegate

        private void JumpStateStart()
        {
            doubleJump = !doubleJump;

            HandleApplyJumpForce(jumpForce);
            animatorTriggerHandler.SetTrigger(jumpParameterHash, 0.1f);
        }

        private void JumpStateUpdate()
        {
            HandleJump(moveInput, lookInput);
        }

        protected abstract void HandleApplyJumpForce(float jumpForce);

        protected abstract void HandleJump(Vector2 moveInput, Vector3 lookInput);

        #endregion

    }
}