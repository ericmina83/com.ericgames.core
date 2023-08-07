using System;
using UnityEngine;


namespace EricGames.Runtime.Mechanics
{
    [RequireComponent(typeof(Collider2D))]
    public class Hittable : MonoBehaviour
    {
        public delegate Skill HittablePreprocessorEvent(Skill damage);

        [SerializeField]
        private event HittablePreprocessorEvent preprocessorEvent;
        public event HittablePreprocessorEvent PreprocessorEvent
        {
            add { this.preprocessorEvent += value; }
            remove { this.preprocessorEvent -= value; }
        }

        private bool untouchable = false;
        public bool Untouchable
        {
            set { untouchable = value; }
            get { return untouchable; }
        }

        [SerializeField]
        private event Action<Skill> hitEvent;
        public event Action<Skill> HitEvent
        {
            add { this.hitEvent += value; }
            remove { this.hitEvent -= value; }
        }

        public void ApplyEffect(Skill skill)
        {
            if (hitEvent != null)
            {
                hitEvent.Invoke(skill);
            }
        }
    }
}