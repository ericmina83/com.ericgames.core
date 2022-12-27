using UnityEngine;

namespace EricGames.Core.Utility
{
    static public class AnimatorFunctions
    {
        static public bool CheckCurrentAndNextStateIs(this Animator animator, int layer, int stateTagHash)
        {
            if (animator.IsInTransition(layer) && animator.GetNextAnimatorStateInfo(layer).tagHash == stateTagHash)
            {
                return true;
            }
            else if (animator.GetCurrentAnimatorStateInfo(layer).tagHash == stateTagHash)
            {
                return true;
            }

            return false;
        }

        static public int GetCurrentStateTagHash(this Animator animator, int layer)
        {
            return animator.GetCurrentAnimatorStateInfo(layer).tagHash;
        }

        static public bool CheckCurrentStateIs(this Animator animator, int layer, int stateTagHash)
        {
            return !animator.IsInTransition(layer)
                && animator.GetCurrentAnimatorStateInfo(layer).tagHash == stateTagHash;
        }
    }
}