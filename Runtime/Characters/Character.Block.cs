using UnityEngine;
using EricGames.Core.StateMachine;

namespace EricGames.Core.Characters
{
    public partial class Character
    {
        private void InitStateBlock()
        {
            var blockState = stateMachine.GetSubState(State.BLOCK);

            blockState.ReigsterStateDelegate(State.BLOCK, StateDelegateType.START, BlockStateStart);
            blockState.ReigsterStateDelegate(State.BLOCK, StateDelegateType.UPDATE, BlockStateUpdate);
            blockState.ReigsterStateDelegate(State.BLOCK, StateDelegateType.END, BlockStateEnd);

            blockState.RegisterTransition(State.BLOCK, State.MOVE, 0.0f,
                null,
                () => !blocking);
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
        }

        private void BlockStateUpdate()
        {
            blockingTime += Time.deltaTime;
        }

        private void BlockStateEnd()
        {
            blockingTime = 0.0f;
            animator.SetBool(blockParameterHash, false);
        }

        #endregion
    }
}