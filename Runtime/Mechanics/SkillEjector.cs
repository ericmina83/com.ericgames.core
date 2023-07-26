using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace EricGames.Core.Mechanics
{
    public class SkillEjector : MonoBehaviour
    {
        public void OnEjectSkill(Skill skill)
        {
            if (skill != null)
            {
                var results = new List<Collider2D>();
                Vector2 skillCenter = (Vector2)(transform.position + transform.right * skill.HitBoxCenter.x + transform.up * skill.HitBoxCenter.y);
                if (Physics2D.OverlapBox(
                    skillCenter,
                    skill.HitBoxSize,
                    skill.HitBoxAngle,
                    new ContactFilter2D()
                    {
                        useLayerMask = true,
                        layerMask = LayerMask.GetMask("Character"),
                    },
                    results) > 0)
                {
                    var result = results.First();
                    if (result.TryGetComponent<Hittable>(out var hittable))
                    {
                        Debug.Log(hittable.name);
                        hittable.ApplyEffect(skill);
                    }
                }
            }
        }
    }
}
