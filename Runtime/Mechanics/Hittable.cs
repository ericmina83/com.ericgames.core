using System;
using UnityEngine;

namespace EricGames.Runtime.Mechanics
{
    [RequireComponent(typeof(Collider2D))]
    public class Hittable : MonoBehaviour
    {
        protected bool untouchable = false;
        public virtual bool Untouchable
        {
            set => untouchable = value;
            get => untouchable;
        }

        [SerializeField]
        protected SkillEjector source = null;
        public virtual SkillEjector Source
        {
            get => source;
            set => source = value;
        }

        [SerializeField]
        private event Action<Skill> hitEvent;
        public event Action<Skill> HitEvent
        {
            add => hitEvent += value;
            remove => hitEvent -= value;
        }

        public void ApplyEffect(Skill skill)
        {
            if (untouchable) return;
            if (skill.source == source) return;
            Debug.Log(gameObject.name);
            hitEvent?.Invoke(skill);
        }
    }
}