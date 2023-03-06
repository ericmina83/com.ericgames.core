using UnityEngine;
using EricGames.Core.StateMachine;

namespace EricGames.Core.Characters
{
    public partial class Character
    {
        private enum MovementState
        {
            MOVE,
            FALL,
            JUMP
        }

        private StateMachine<MovementState> movementStateMachine;

        private void InitStateMovement()
        {
            // init state
            var movementState = stateMachine.GetSubState(State.MOVEMENT);

            movementState.ReigsterStateDelegate(StateDelegateType.START, MovementStateStart);
            movementState.ReigsterStateDelegate(StateDelegateType.UPDATE, MovementStateUpdate);

            movementState.RegisterTransition(State.DODGE, 0.2f,
                () => triggerHandler.GetTriggerValue(TriggerType.DODGE));
            movementState.RegisterTransition(State.ATTACK, 0.2f,
                () => triggerHandler.GetTriggerValue(TriggerType.ATTACK));
            movementState.RegisterTransition(State.BLOCK, 0.2f,
                () => blocking);

            // init sub state machine
            movementStateMachine = new StateMachine<MovementState>(MovementState.MOVE);

            movementStateMachine.RegisterStartTransition(MovementState.MOVE,
                () => landingState == LandingState.GROUNDED);
            movementStateMachine.RegisterStartTransition(MovementState.FALL,
                () => landingState == LandingState.FALLING);
            movementStateMachine.RegisterStartTransition(MovementState.JUMP,
                () => landingState == LandingState.JUMPING);

            InitStateMove();
            InitStateFall();
            InitStateJump();
        }

        #region State Delegate

        private void MovementStateStart()
        {
            movementStateMachine.Reset();
        }

        private void MovementStateUpdate()
        {
            movementStateMachine.Tick(Time.deltaTime);
        }

        #endregion
    }
}