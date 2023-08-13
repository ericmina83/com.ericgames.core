using EricGames.Runtime.Characters;
using UnityEngine;

namespace EricGames.Runtime.Mechanics
{
    public class BlockMechanicsHandler : MonoBehaviour
    {
        [SerializeField]
        private MechanicsDamageHandler damageHandler;

        [SerializeField]
        private Character character;

        private bool blocking = false;

        private void OnValidate()
        {
            if (damageHandler == null)
            {
                Debug.LogError(
                    $"[BlockMechanicsHandler on {gameObject.name}]: `damageHandler` hasn't been set.");
            }

            if (character == null)
            {
                Debug.LogError(
                    $"[BlockMechanicsHandler on {gameObject.name}]: `Character` hasn't been set.");
            }
        }

        private void OnEnable()
        {
            if (damageHandler != null)
            {
                damageHandler.PostprocessingEvent += OnBlockEffect;
            }

            if (character != null)
            {
                character.OnBlockStartEvent += OnBlockStart;
                character.OnBlockEndEvent += OnBlockEnd;
            }
        }

        private void OnDisable()
        {
            if (damageHandler != null)
            {
                damageHandler.PostprocessingEvent -= OnBlockEffect;
            }

            if (character != null)
            {
                character.OnBlockStartEvent -= OnBlockStart;
                character.OnBlockEndEvent -= OnBlockEnd;
            }
        }

        private void OnBlockStart() => blocking = true;
        private void OnBlockEnd() => blocking = false;

        private Skill OnBlockEffect(Skill skill)
        {
            if (blocking)
            {
                skill.SetBlocked();
            }
            return skill;
        }
    }
}