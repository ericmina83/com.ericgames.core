using EricGames.Core.StateMachine;
using UnityEngine;

namespace EricGames.Core.Characters
{
    public partial class Character
    {
        private void InitStateFall()
        {
            var fallState = stateMachine.GetSubState(State.FALL);

            fallState.ReigsterStateDelegate(StateDelegateType.UPDATE, FallStateUpdate);

            fallState.RegisterTransition(State.MOVE, 0.0f,
                null,
                () => landingState == LandingState.GROUNDED);
            fallState.RegisterTransition(State.DODGE, 0.0f,
                new TriggerType[] { TriggerType.DODGE },
                null);
            fallState.RegisterTransition(State.ATTACK, 0.0f,
                new TriggerType[] { TriggerType.ATTACK },
                null);
            fallState.RegisterTransition(State.JUMP, 0.0f,
                new TriggerType[] { TriggerType.JUMP },
                () => landingState == LandingState.GROUNDED ? true : doubleJump);
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