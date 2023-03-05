using EricGames.Core.StateMachine;
using UnityEngine;

namespace EricGames.Core.Characters
{
    public partial class Character
    {
        private void InitStateFall()
        {
            var fallState = stateMachine.GetSubState(State.FALL);

            fallState.ReigsterStateDelegate(State.FALL, StateDelegateType.UPDATE, FallStateUpdate);

            fallState.RegisterTransition(State.FALL, State.MOVE, 0.0f,
                null,
                () => landingState == LandingState.GROUNDED);
            fallState.RegisterTransition(State.FALL, State.DODGE, 0.0f,
                new TriggerType[] { TriggerType.DODGE },
                null);
            fallState.RegisterTransition(State.FALL, State.ATTACK, 0.0f,
                new TriggerType[] { TriggerType.ATTACK },
                null);
            fallState.RegisterTransition(State.FALL, State.JUMP, 0.0f,
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