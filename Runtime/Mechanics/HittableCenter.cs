using UnityEngine;

namespace EricGames.Runtime.Mechanics
{
    public sealed class HittableCenter : Hittable
    {
        [SerializeField]
        private Hittable[] hittableParts;

        public override bool Untouchable
        {
            get => untouchable;
            set
            {
                untouchable = value;
                foreach (var hittable in hittableParts)
                {
                    if (hittable == this) continue;
                    hittable.Untouchable = untouchable;
                }
            }
        }

        private void OnEnable()
        {
            foreach (var hittable in hittableParts)
            {
                if (hittable == this) continue;
                hittable.HitEvent += ApplyEffect;
            }
        }
    }
}