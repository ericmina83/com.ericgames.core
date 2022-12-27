using EricGames.Core.StateMachine;

namespace EricGames.Core.Characters
{
    public partial class Character
    {
        private void InitStateFall()
        {
            stateMachine.ReigsterStateDelegate(State.FALL, StateDelegateType.UPDATE, FallStateUpdate);

            stateMachine.RegisterTransition(State.FALL, State.MOVE, 0.0f,
                null,
                () => landingState == LandingState.GROUNDED);
            stateMachine.RegisterTransition(State.FALL, State.DODGE, 0.0f,
                new TriggerType[] { TriggerType.DODGE },
                null);
            stateMachine.RegisterTransition(State.FALL, State.ATTACK, 0.0f,
                new TriggerType[] { TriggerType.ATTACK },
                null);
            stateMachine.RegisterTransition(State.FALL, State.JUMP, 0.0f,
                new TriggerType[] { TriggerType.JUMP },
                () => landingState == LandingState.GROUNDED ? true : doubleJump);
        }

        #region State Delegate

        private void FallStateUpdate()
        {
            CheckRotation(targMoveInput.x);
        }

        #endregion
    }
}