using UnityEngine;
using EricGames.Runtime.StateMachine;

namespace EricGames.Runtime.Characters
{
    public partial class Character
    {
        public bool blocking = false;

        public delegate void OnBlockDelegate();

        private event OnBlockDelegate onBlockStartEvent;
        public event OnBlockDelegate OnBlockStartEvent
        {
            add => onBlockStartEvent += value;
            remove => onBlockStartEvent -= value;
        }

        private event OnBlockDelegate onBlockEndEvent;
        public event OnBlockDelegate OnBlockEndEvent
        {
            add => onBlockEndEvent += value;
            remove => onBlockEndEvent -= value;
        }

        private void InitStateBlock()
        {
            var hitState = stateMachine.GetSubState(State.BLOCK);

            hitState.StateStartEvent += BlockStateStart;
            hitState.StateUpdateEvent += BlockStateUpdate;
            hitState.StateEndEvent += BlockStateEnd;

            hitState.RegisterTransition(State.MOVEMENT, 0.2f,
                () => !blocking);

            hitState.RegisterTransition(State.MOVEMENT, 0.2f,
                () => landingState != LandingState.GROUNDED);
        }

        #region Trigger Function

        virtual public void Block(bool enabled)
        {
            blocking = enabled;
        }

        #endregion

        #region State Delegate

        private void BlockStateStart()
        {
            blockingTime = 0.0f;
            animator.SetBool(blockParameterHash, true);
            onBlockStartEvent.Invoke();
        }

        private void BlockStateUpdate()
        {
            blockingTime += Time.deltaTime;
        }

        private void BlockStateEnd()
        {
            blockingTime = 0.0f;
            animator.SetBool(blockParameterHash, false);
            onBlockEndEvent.Invoke();
        }

        #endregion
    }
}