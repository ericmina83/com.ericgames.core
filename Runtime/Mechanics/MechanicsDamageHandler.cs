using System;
using UnityEngine;

namespace EricGames.Runtime.Mechanics
{
    public class MechanicsDamageHandler : MonoBehaviour
    {
        [SerializeField]
        private Hittable hittable;

        public delegate Skill HittablePreprocessorEvent(Skill damage);

        private event HittablePreprocessorEvent postprocessingEvent;
        public event HittablePreprocessorEvent PostprocessingEvent
        {
            add => postprocessingEvent += value;
            remove => postprocessingEvent -= value;
        }

        [SerializeField]
        private event Action<Skill> damageEvent;
        public event Action<Skill> DamageEvent
        {
            add => damageEvent += value;
            remove => damageEvent -= value;
        }

        private void OnEnable()
        {
            hittable.HitEvent += OnHitEvent;
        }

        private void OnDisable()
        {
            hittable.HitEvent -= OnHitEvent;
        }

        private void OnHitEvent(Skill skill)
        {
            if (postprocessingEvent != null)
            {
                foreach (HittablePreprocessorEvent postprocessing in postprocessingEvent.GetInvocationList())
                {
                    skill = postprocessing(skill);
                }
            }

            damageEvent?.Invoke(skill);
        }
    }
}