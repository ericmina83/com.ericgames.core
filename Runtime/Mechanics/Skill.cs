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
        private Vector2 hitBoxSize;
        public Vector2 HitBoxSize => hitBoxSize;

        [SerializeField]
        private float hitBoxAngle;
        public float HitBoxAngle => hitBoxAngle;

        [SerializeField]
        private int frames;
        public int Frames => frames;

        public SkillEjector owner;

        public bool enable; // damage effect or not
        public int damageAmount;
        public Vector3 attackFrom;

    }
}