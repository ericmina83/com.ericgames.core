using UnityEngine;
using EricGames.Core.Mechanics;
using EricGames.Core.Characters;

namespace EricGames.Core.Equipment
{
    public class Weapon : MonoBehaviour
    {
        public Character owner;

        public void Awake()
        {
            owner = GetComponentInParent<Character>();
        }
    }
}