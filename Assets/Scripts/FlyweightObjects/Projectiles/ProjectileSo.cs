using BestGameEver.FlyweightObjects.Base;
using UnityEngine;

namespace BestGameEver.FlyweightObjects.Projectiles
{
    [CreateAssetMenu(fileName = "new Projectile", menuName = "Scriptable Objects/Projectile", order = 0)]
    public sealed class ProjectileSo : FlyweightSo
    {
        [field: SerializeField] public float Speed{ get; private set; }
        [field: SerializeField] public int EffectAmount{ get; private set; }
        [field: SerializeField] public float Lifetime{ get; private set; }

        public override Flyweight CreateProjectile()
        {
            GameObject obj = Instantiate(Prefab);
            obj.SetActive(false);
            
            var projectile = obj.GetComponent<Projectile>();
            projectile.settings = this;
            
            return projectile;
        }
    }
}