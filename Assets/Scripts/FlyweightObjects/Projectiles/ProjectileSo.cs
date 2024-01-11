using BestGameEver.FlyweightObjects.Base;
using UnityEngine;

namespace BestGameEver.FlyweightObjects.Projectiles
{
    [CreateAssetMenu(fileName = "new Projectile", menuName = "Scriptable Objects/Projectile", order = 0)]
    public sealed class ProjectileSo : FlyweightSo
    {
        public float speed;
        public int effectAmount;
        public float lifetime;

        public override Flyweight CreateProjectile()
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            
            Projectile projectile = obj.GetComponent<Projectile>();
            projectile.settings = this;
            
            return projectile;
        }
    }
}