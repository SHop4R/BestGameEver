using UnityEngine;

namespace BestGameEver.FlyweightObjects.Base
{
    public abstract class FlyweightSo : ScriptableObject
    {
        public FlyweightObjectType type;
        public GameObject prefab;
        
        public virtual Flyweight CreateProjectile()
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);

            obj.TryGetComponent(out Flyweight flyweight);
            flyweight.settings = this;
            
            return flyweight;
        }
        
        public virtual void OnGetProjectile(Flyweight projectile) => projectile.gameObject.SetActive(true);
        
        public virtual void OnReleaseProjectile(Flyweight projectile) => projectile.gameObject.SetActive(false);
        
        public virtual void DestroyProjectile(Flyweight projectile) => Destroy(projectile.gameObject);
    }
}