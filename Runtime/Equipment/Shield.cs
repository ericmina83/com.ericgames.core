using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EricGames.Core.Equipment
{
    public class Shield : MonoBehaviour
    {
        [Range(0, 1)] public float shieldAlpha = 1.0f;
        public Color color;
        Material material;

        // Start is called before the first frame update
        void Start()
        {
            material = GetComponent<SpriteRenderer>().material;
        }

        // Update is called once per frame
        void Update()
        {
            material.SetFloat("_Alpha", shieldAlpha);
        }
    }
}
