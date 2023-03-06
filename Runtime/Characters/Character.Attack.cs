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

            attackState.ReigsterStateDelegate(StateDelegateType.START, AttackStateStart);
            attackState.ReigsterStateDelegate(StateDelegateType.UPDATE, AttackStateUpdate);

            attackState.RegisterTransition(State.ATTACK, 0f,
                () => canDoNext && triggerHandler.GetTriggerValue(TriggerType.ATTACK));
            attackState.RegisterTransition(State.MOVEMENT, 0f,
                () => !animator.CheckCurrentStateIs(0, attackStateTagHash));
        }

        #region Trigger Function

        public void Attack()
        {
            triggerHandler.SetTrigger(TriggerType.ATTACK, 0.4f);
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

            triggerHandler.ResetTrigger(TriggerType.ATTACK);
            animatorTriggerHandler.SetTrigger(attackParameterHash, 0.4f);
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