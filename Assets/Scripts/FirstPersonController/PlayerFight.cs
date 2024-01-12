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
        
        protected override void Update()
        {
            base.Update();

            if (StarterInputs.fireLeft)
            {
                StarterInputs.fireLeft = false;
                SpawnProjectile(FlyweightObjectType.DamageProjectile);
            }
            
            if (StarterInputs.fireRight)
            {
                StarterInputs.fireRight = false;
                SpawnProjectile(FlyweightObjectType.HealProjectile);
            }
        }
        
        private void SpawnProjectile(FlyweightObjectType type)
        {
            if (Time.time - _lastFireRate < fireRate) return;
            
            Flyweight obj = FlyweightFactory.Instance.Spawn(type);
            if (!obj.TryGetComponent(out Transform objectTransform)) return;
            
            objectTransform.position = cinemachineCameraTarget.position;
            objectTransform.rotation = cinemachineCameraTarget.rotation;
            
            _lastFireRate = Time.time;
        }
    }
}