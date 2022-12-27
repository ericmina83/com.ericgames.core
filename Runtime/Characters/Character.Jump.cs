using UnityEngine;
using EricGames.Core.StateMachine;

namespace EricGames.Core.Characters
{
    public partial class Character
    {
        private void InitStateJump()
        {
            stateMachine.ReigsterStateDelegate(State.JUMP, StateDelegateType.START, JumpStateStart);
            stateMachine.ReigsterStateDelegate(State.JUMP, StateDelegateType.UPDATE, JumpStateUpdate);

            stateMachine.RegisterTransition(State.JUMP, State.JUMP, 0.0f,
                new TriggerType[] { TriggerType.JUMP },
                () => landingState == LandingState.GROUNDED ? true : doubleJump);
            stateMachine.RegisterTransition(State.JUMP, State.FALL, 0.0f,
                null,
                () => landingState != LandingState.GROUNDED && speedY < 0);
            stateMachine.RegisterTransition(State.JUMP, State.MOVE, 0.0f,
                null,
                () => landingState == LandingState.GROUNDED);
            stateMachine.RegisterTransition(State.JUMP, State.DODGE, 0.0f,
                new TriggerType[] { TriggerType.DODGE },
                null);
            stateMachine.RegisterTransition(State.JUMP, State.ATTACK, 0.0f,
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

            rb.velocity = new Vector2(0, jumpForce);
            animatorTriggerHandler.SetTrigger(jumpParameterHash, 0.1f);
        }

        private void JumpStateUpdate()
        {
            CheckRotation(targMoveInput.x);
        }

        #endregion

    }
}