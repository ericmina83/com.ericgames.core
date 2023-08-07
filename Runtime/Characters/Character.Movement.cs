using UnityEngine;
using EricGames.Runtime.StateMachine;

namespace EricGames.Runtime.Characters
{
    public partial class Character
    {
        private enum MovementState
        {
            MOVE,
            FALL,
            JUMP
        }

        [SerializeField] protected float moveSpeed = 1.0f;
        [SerializeField] protected float jumpForce = 7.0f; // decide how height when jumping

        protected float SpeedX => Mathf.Abs(moveInput.x);
        abstract protected float SpeedY { get; }

        private readonly StateMachine<MovementState> movementStateMachine = new(MovementState.MOVE);

        private void InitStateMovement()
        {
            // init state
            var movementState = stateMachine.GetSubState(State.MOVEMENT);

            movementState.RegisterStateDelegate(StateDelegateType.START, MovementStateStart);
            movementState.RegisterStateDelegate(StateDelegateType.UPDATE, MovementStateUpdate);

            movementState.RegisterTransition(State.DODGE, 0.2f,
                () => triggerHandler.GetTriggerValue(TriggerType.DODGE));
            movementState.RegisterTransition(State.ATTACK, 0.2f,
                () => triggerHandler.GetTriggerValue(TriggerType.ATTACK));
            movementState.RegisterTransition(State.BLOCK, 0.2f,
                () => blocking);

            // init sub state machine
            InitStateMove();
            InitStateFall();
            InitStateJump();
        }

        private void ApplyRotation()
        {
            var magnitude = moveInput.magnitude;

            if (!Mathf.Approximately(magnitude, 0.0f))
            {
                transform.rotation = CheckRotation(transform.rotation, moveInput);
            }
        }

        // check current rotation is same as input
        public abstract Quaternion CheckRotation(Quaternion currentRotation, Vector2 input);

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