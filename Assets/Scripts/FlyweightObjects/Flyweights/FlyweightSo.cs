using BestGameEver.FlyweightObjects.Base;
using UnityEngine;

namespace BestGameEver.FlyweightObjects.Flyweights
{
    public abstract class FlyweightSo : ScriptableObject
    {
        [field: SerializeField] public FlyweightObjectType ObjectType{ get; private set; }
        [field: SerializeField] public GameObject Prefab{ get; private set; }
        
        public Flyweight CreateProjectile()
        {
            GameObject obj = Instantiate(Prefab);
            obj.SetActive(false);

            obj.TryGetComponent(out Flyweight component);
            component.settings = this;
            
            return component;
        }
        
        public static void OnGetProjectile(Flyweight projectile) => projectile.gameObject.SetActive(true);
        
        public static void OnReleaseProjectile(Flyweight projectile) => projectile.gameObject.SetActive(false);
        
        public static void DestroyProjectile(Flyweight projectile) => Destroy(projectile.gameObject);
    }
}