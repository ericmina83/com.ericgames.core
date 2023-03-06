using EricGames.Core.StateMachine;

namespace EricGames.Core.Characters
{
    public partial class Character
    {
        private enum AttackState
        {
            WAIT,
            PREPARING,
            ATTACKING,
            RESTORING,
            END
        }


        private AttackState attackSubState = AttackState.PREPARING;

        private void InitStateAttack()
        {
            var attackState = stateMachine.GetSubState(State.ATTACK);

            attackState.ReigsterStateDelegate(StateDelegateType.START, AttackStateStart);
            attackState.ReigsterStateDelegate(StateDelegateType.UPDATE, AttackStateUpdate);

            attackState.RegisterTransition(State.ATTACK, 0f,
                () => attackSubState == AttackState.RESTORING && triggerHandler.GetTriggerValue(TriggerType.ATTACK));
            attackState.RegisterTransition(State.MOVEMENT, 0f,
                () => attackSubState == AttackState.END);
        }

        #region Trigger Function

        public void Attack()
        {
            triggerHandler.SetTrigger(TriggerType.ATTACK, 0.4f);
        }

        #endregion

        public void AttackPrepare()
        {
            attackSubState = AttackState.PREPARING;
        }

        public void AttackStart()
        {
            attackSubState = AttackState.ATTACKING;
        }

        public void AttackRestore()
        {
            attackSubState = AttackState.RESTORING;
        }

        public void AttackEnd()
        {
            if (attackSubState == AttackState.RESTORING)
            {
                attackSubState = AttackState.END;
                triggerHandler.ResetTrigger(TriggerType.ATTACK);
            }
        }

        #region State Delegate

        private void AttackStateStart()
        {
            attackSubState = AttackState.WAIT;

            triggerHandler.ResetTrigger(TriggerType.ATTACK);
            animatorTriggerHandler.SetTrigger(attackParameterHash, 0.4f);
        }

        public void AttackStateUpdate()
        {
            if (attackSubState == AttackState.PREPARING)
            {
                ApplyRotation();
                HandlePreparing();
            }
            else if (attackSubState == AttackState.ATTACKING)
            {
                HandleAttacking();
            }
            else if (attackSubState == AttackState.RESTORING)
            {
                HandleRestoring();
            }
        }

        #endregion

        #region Abastract Function

        public abstract void HandlePreparing();
        public abstract void HandleAttacking();
        public abstract void HandleRestoring();

        #endregion
    }
}