using EricGames.Core.StateMachine;
using UnityEngine;

namespace EricGames.Core.Characters
{
    public partial class Character
    {
        private void InitStateFall()
        {
            var fallState = movementStateMachine.GetSubState(MovementState.FALL);

            fallState.ReigsterStateDelegate(StateDelegateType.UPDATE, FallStateUpdate);

            fallState.RegisterTransition(MovementState.MOVE, 0.0f,
                () => landingState == LandingState.GROUNDED);
            fallState.RegisterTransition(MovementState.JUMP, 0.0f,
                () => landingState == LandingState.GROUNDED ? true : doubleJump
                    && triggerHandler.GetTriggerValue(TriggerType.JUMP));
        }

        #region State Delegate

        private void FallStateUpdate()
        {
            ApplyRotation();

            HandleFall(moveInput, lookInput);
        }

        #endregion

        #region Abstract Functions

        protected abstract void HandleFall(Vector2 moveInput, Vector3 lookInput);

        #endregion
    }
}