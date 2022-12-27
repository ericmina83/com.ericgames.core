using System.Collections.Generic;
using UnityEngine;

namespace EricGames.Core.Components
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorTriggerHandler : MonoBehaviour
    {
        private Animator animator = null;
        Dictionary<int, float> timers = new Dictionary<int, float>();

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            var keys = new List<int>(timers.Keys); // copy keys list becuase the dicionary will change later

            foreach (var triggerId in keys)
            {
                timers[triggerId] -= Time.deltaTime;
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