using BestGameEver.Factory;
using BestGameEver.FlyweightObjects.Base;
using UnityEngine;

namespace BestGameEver.FirstPersonController
{
    [SelectionBase, DisallowMultipleComponent]
    public sealed class PlayerFight : FirstPersonController
    {
        [Header("Fire Rate")]
        [SerializeField] private float fireRate;
        private float _lastFireRate;

        private FlyweightFactory _factory;
        
        protected override void Awake()
        {
            base.Awake();
            
            _factory = FlyweightFactory.Instance;
        }
        
        protected override void Update()
        {
            base.Update();

            if (StarterInputs.fireLeft)
            {
                SpawnProjectile(FlyweightObjectType.DamageProjectile);
                StarterInputs.fireLeft = false;
            }
            
            if (StarterInputs.fireRight)
            {
                SpawnProjectile(FlyweightObjectType.HealProjectile);
                StarterInputs.fireRight = false;
            }
        }
        
        private void SpawnProjectile(FlyweightObjectType type)
        {
            if (Time.time - _lastFireRate < fireRate) return;
            
            Flyweight obj = _factory.Spawn(type);
            if (!obj.TryGetComponent(out Transform objectTransform)) return;
            
            objectTransform.position = cinemachineCameraTarget.position;
            objectTransform.rotation = cinemachineCameraTarget.rotation;
            
            _lastFireRate = Time.time;
        }
    }
}