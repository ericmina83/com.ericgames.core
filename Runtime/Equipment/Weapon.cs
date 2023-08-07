using UnityEngine;
using EricGames.Runtime.Mechanics;
using EricGames.Runtime.Characters;

namespace EricGames.Runtime.Equipment
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