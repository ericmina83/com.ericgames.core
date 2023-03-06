using UnityEngine;
using EricGames.Core.StateMachine;

namespace EricGames.Core.Characters
{
    public partial class Character
    {
        private void InitStateBlock()
        {
            var blockState = stateMachine.GetSubState(State.BLOCK);

            blockState.ReigsterStateDelegate(StateDelegateType.START, BlockStateStart);
            blockState.ReigsterStateDelegate(StateDelegateType.UPDATE, BlockStateUpdate);
            blockState.ReigsterStateDelegate(StateDelegateType.END, BlockStateEnd);

            blockState.RegisterTransition(State.MOVEMENT, 0.2f,
                () => !blocking);

            blockState.RegisterTransition(State.MOVEMENT, 0.2f,
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