using UnityEngine;
using EricGames.Core.Mechanics;
using EricGames.Core.Characters;

namespace EricGames.Core.Equipment
{
    public class Weapon : MonoBehaviour
    {
        private Damage damage = null;
        public Characters.Character owner;

        public void SetDamage(Damage damage)
        {
            this.damage = damage;
        }

        // Start is called before the first frame update
        void OnTriggerStay2D(Collider2D other)
        {
            if (damage == null)
                return;

            if (!damage.enable)
                return;

            var body = other.GetComponent<Body>();

            if (body == null) // if there is no body, return
                return;

            Characters.Character enemy = body.GetOwner();

            if (enemy == owner)
                return;

            if (enemy.team == owner.team)
                return;

            damage.HandleDamage(enemy);
        }
    }
}