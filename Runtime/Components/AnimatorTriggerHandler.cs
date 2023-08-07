using System.Collections.Generic;
using UnityEngine;

namespace EricGames.Runtime.Components
{
    public class AnimatorTriggerHandler
    {
        private readonly Dictionary<int, float> timers = new();
        private readonly Animator animator;

        public AnimatorTriggerHandler(Animator animator)
        {
            this.animator = animator;
        }

        public void Tick(float deltaTime)
        {
            var keys = new List<int>(timers.Keys);
            // copy keys list because the dictionary will be changed later.

            foreach (var triggerId in keys)
            {
                timers[triggerId] -= deltaTime;
                if (timers[triggerId] < 0.0f)
                {
                    animator.ResetTrigger(triggerId);
                    timers.Remove(triggerId);
                }
            }
        }

        public void SetTrigger(int triggerHash, float countdownTime)
        {
            animator.SetTrigger(triggerHash);
            timers[triggerHash] = countdownTime;
        }
    }
}