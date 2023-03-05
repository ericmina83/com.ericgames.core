using EricGames.Core.StateMachine;
using EricGames.Core.Utility;

namespace EricGames.Core.Characters
{
    public partial class Character
    {
        private enum AttackState
        {
            PREPARE,
            ATTACKING,
            RESTORE
        }

        private bool canDoNext = false;
        private bool preAttack = false;

        private void InitStateAttack()
        {
            var attackState = stateMachine.GetSubState(State.ATTACK);

            attackState.ReigsterStateDelegate(State.ATTACK, StateDelegateType.START, AttackStateStart);
            attackState.ReigsterStateDelegate(State.ATTACK, StateDelegateType.UPDATE, AttackStateUpdate);

            attackState.RegisterTransition(State.ATTACK, State.ATTACK, 0f,
                new TriggerType[] { TriggerType.ATTACK },
                () => canDoNext);
            attackState.RegisterTransition(State.ATTACK, State.MOVE, 0.0f,
                null,
                () => animator.CheckCurrentStateIs(0, moveStateTagHash) && landingState == LandingState.GROUNDED);
            attackState.RegisterTransition(State.ATTACK, State.FALL, 0.0f,
                null,
                () => animator.CheckCurrentStateIs(0, moveStateTagHash) && landingState == LandingState.FALLING);
            attackState.RegisterTransition(State.ATTACK, State.JUMP, 0.0f,
                null,
                () => animator.CheckCurrentStateIs(0, moveStateTagHash) && landingState == LandingState.JUMPING);
        }

        #region Trigger Function

        public void Attack()
        {
            stateMachine.SetTrigger(TriggerType.ATTACK, 0.4f);
            animatorTriggerHandler.SetTrigger(attackParameterHash, 0.4f);
        }

        #endregion

        public void CanDoNext()
        {
            canDoNext = true;
        }

        public void CanDoNextEnd()
        {
            canDoNext = false;
        }

        public void PreAttackStart()
        {
            preAttack = true;
        }

        public void PreAttackEnd()
        {
            preAttack = false;
        }

        #region State Delegate

        private void AttackStateStart()
        {
            canDoNext = false;
            preAttack = false;
        }

        public void AttackStateUpdate()
        {
            if (animator.IsInTransition(0))
            {
                ApplyRotation();
            }
            else
            {
                HandleAttack();
            }
        }

        #endregion

        #region Abastract Function

        public abstract void HandleAttack();

        #endregion
    }
}