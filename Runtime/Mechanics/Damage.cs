using UnityEngine;
using EricGames.Core.Characters;

namespace EricGames.Core.Mechanics
{
    public class Damage
    {

        public bool enable; // damage effect or not
        public int damageAmount;
        public Vector3 attackFrom;
        public Character owner;

        public Damage(Character owner)
        {
            damageAmount = 30;
            this.owner = owner;
        }

        public void HandleDamage(Character enemy)
        {
            // init
            attackFrom = enemy.transform.position - owner.transform.position;

            enemy.HandleHitted(this);

            // 
            enable = false;
        }
    }
}