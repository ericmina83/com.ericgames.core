using EricGames.Runtime.StateMachine;
using UnityEngine;

namespace EricGames.Runtime.Characters
{
    public partial class Character
    {
        private void InitStateFall()
        {
            var fallState = movementStateMachine.GetSubState(MovementState.FALL);

            fallState.StateUpdateEvent += FallStateUpdate;

            fallState.RegisterTransition(MovementState.MOVE, 0.0f,
                () => landingState == LandingState.GROUNDED);
            fallState.RegisterTransition(MovementState.JUMP, 0.0f,
                () => doubleJump && triggerHandler.GetTriggerValue(TriggerType.JUMP));
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