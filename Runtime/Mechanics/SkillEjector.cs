using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace EricGames.Runtime.Mechanics
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
                        layerMask = skill.TargetLayer,
                    },
                    results) > 0)
                {
                    foreach (var hit in results)
                    {
                        if (hit.TryGetComponent<Hittable>(out var hittable))
                        {
                            var cloneSkill = Instantiate(skill);
                            cloneSkill.source = this;
                            hittable.ApplyEffect(cloneSkill);
                        }
                    }
                }
            }
        }
    }
}
