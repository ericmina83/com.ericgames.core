using UnityEngine;

namespace EricGames.Core.Characters
{
    [RequireComponent(typeof(Collider2D))]
    public class Body : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        private Character owner;
        Collider2D myCollider;

        public void SetOwner(Character owner)
        {
            this.owner = owner;
        }

        public Character GetOwner()
        {
            return owner;
        }

        public void Start()
        {
            myCollider = GetComponent<Collider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if (owner.untouchable)
            {
                myCollider.enabled = false;
            }
            else
            {
                myCollider.enabled = true;
            }
        }
    }
}