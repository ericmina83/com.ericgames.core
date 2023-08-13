using EricGames.Runtime.StateMachine;
using UnityEngine;

namespace EricGames.Runtime.Characters
{
    public partial class Character
    {
        private enum AttackState
        {
            WAIT,
            PREPARING,
            ATTACKING,
            RESETTING,
            ENDING
        }

        public delegate void AttackDelegate();

        public event AttackDelegate OnPreparing;
        public event AttackDelegate OnAttacking;
        public event AttackDelegate OnResetting;

        private AttackState attackSubState = AttackState.PREPARING;

        private void InitStateAttack()
        {
            var attackState = stateMachine.GetSubState(State.ATTACK);

            attackState.StateStartEvent += AttackStateStart;
            attackState.StateUpdateEvent += AttackStateUpdate;

            attackState.RegisterTransition(State.ATTACK, null,
                () => attackSubState == AttackState.RESETTING
                    && triggerHandler.GetTriggerValue(TriggerType.ATTACK));
            attackState.RegisterTransition(State.MOVEMENT, null,
                () => attackSubState == AttackState.ENDING);
        }

        #region Trigger Function

        public void Attack()
        {
            triggerHandler.SetTrigger(TriggerType.ATTACK, 0.4f);
        }

        #endregion

        public void OnPreparingAnimationEvent()
        {
            attackSubState = AttackState.PREPARING;
        }

        public void OnAttackingAnimationEvent()
        {
            attackSubState = AttackState.ATTACKING;
        }

        public void OnResettingAnimationEvent()
        {
            attackSubState = AttackState.RESETTING;
        }

        public void OnEndingAnimationEvent()
        {
            if (attackSubState == AttackState.RESETTING)
            {
                attackSubState = AttackState.ENDING;
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
                OnPreparing?.Invoke();
            }
            else if (attackSubState == AttackState.ATTACKING)
            {
                HandleAttacking();
                OnAttacking?.Invoke();
            }
            else if (attackSubState == AttackState.RESETTING)
            {
                HandleResetting();
                OnResetting?.Invoke();
            }
        }

        #endregion

        #region Abastract Function

        protected abstract void HandlePreparing();

        protected abstract void HandleAttacking();

        protected abstract void HandleResetting();

        #endregion
    }
}