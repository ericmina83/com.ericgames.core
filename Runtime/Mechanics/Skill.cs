using System;
using UnityEngine;


namespace EricGames.Runtime.Mechanics
{
    [CreateAssetMenu(menuName = "My Assets/Skill")]
    public class Skill : ScriptableObject
    {
        [SerializeField]
        private Vector2 hitBoxCenter;
        public Vector2 HitBoxCenter => hitBoxCenter;

        [SerializeField]
        private LayerMask targetLayer;
        public LayerMask TargetLayer => targetLayer;

        [SerializeField]
        private Vector2 hitBoxSize;
        public Vector2 HitBoxSize => hitBoxSize;

        [SerializeField]
        private float hitBoxAngle;
        public float HitBoxAngle => hitBoxAngle;

        public SkillEjector source;

        [SerializeField]
        private bool blockable = false;

        [NonSerialized]
        private bool blocked = false;
        public bool Blocked => blocked;

        public void SetBlocked()
        {
            blocked = blockable;
        }

        public int damage;
        public Vector3 attackFrom;
        public bool enable = true; // damage effect or not

    }
}