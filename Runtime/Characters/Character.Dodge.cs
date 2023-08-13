using EricGames.Runtime.StateMachine;

namespace EricGames.Runtime.Characters
{
    public partial class Character
    {
        private bool dodgeEnd = false;

        private void InitStateDodge()
        {
            var dodgeState = stateMachine.GetSubState(State.DODGE);

            dodgeState.StateStartEvent += DodgeStateStart;
            dodgeState.StateEndEvent += DodgeStateEnd;

            dodgeState.RegisterTransition(State.MOVEMENT, 0.0f,
                () => dodgeEnd);
        }

        #region Trigger Function

        public void Dodge()
        {
            triggerHandler.SetTrigger(TriggerType.DODGE, 0.4f);
        }

        #endregion

        #region State Delegate

        private void DodgeStateStart()
        {
            dodgeEnd = false;
            untouchable = true;
            obstacle.SetActive(false);

            triggerHandler.ResetTrigger(TriggerType.DODGE);
            animatorTriggerHandler.SetTrigger(dodgeParameterHash, 0.4f);
        }

        private void DodgeStateEnd()
        {
            dodgeEnd = false;
            untouchable = false;
            obstacle.SetActive(true);
        }

        private void DodgeStart()
        {
        }

        private void DodgeEnd()
        {
            dodgeEnd = true;
        }

        #endregion
    }
}