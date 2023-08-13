using EricGames.Runtime.StateMachine;

namespace EricGames.Runtime.Characters
{
    public partial class Character
    {
        private Mechanics.Hittable hittable;

        #region State HIT

        public void HandleHit()
        {
            if (blocking) // blocking
            {
                //triggerHandler.SetTrigger("Hitted when Blocking", 0.1f);

                // if (Vector3.Dot(damage.attackFrom, transform.right) < 0.0f)

                if (blockingTime < blockingDuration) // perfect blocking
                {
                    // damage.enable = false; // disable
                }
                else // normal blocking
                {
                    // todo 
                }
            }
            else // non-blocking
            {
                //triggerHandler.SetTrigger("Hitted", 0.1f);

                if (animator.IsInTransition(0)) // in transition
                {
                }
                else // in state
                {
                }
            }
        }

        #endregion

        private void InitStateHit()
        {
            var blockState = stateMachine.GetSubState(State.HIT);

            blockState.StateStartEvent += HitStateStart;
            blockState.StateUpdateEvent += HitStateUpdate;
            blockState.StateEndEvent += HitStateEnd;

            blockState.RegisterTransition(State.MOVEMENT, 0.2f,
                () => !blocking);

            blockState.RegisterTransition(State.MOVEMENT, 0.2f,
                () => landingState != LandingState.GROUNDED);

            if (TryGetComponent(out hittable))
            {
                hittable.HitEvent += Hit;
            }
        }

        #region Trigger Function

        virtual protected void Hit(Mechanics.Skill skill)
        {
        }

        #endregion

        #region State Delegate

        private void HitStateStart()
        {
        }

        private void HitStateUpdate()
        {
        }

        private void HitStateEnd()
        {
        }

        #endregion
    }
}