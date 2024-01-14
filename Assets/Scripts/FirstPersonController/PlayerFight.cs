using BestGameEver.Factory;
using BestGameEver.FlyweightObjects.Base;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BestGameEver.FirstPersonController
{
    [SelectionBase, DisallowMultipleComponent]
    public sealed class PlayerFight : FirstPersonController
    {
        [Header("Fire Rate")]
        [SerializeField] private float fireRate;
        private float _lastFireRate;

        public void OnFireLeft(InputValue value)
        {
            SpawnProjectile(FlyweightObjectType.DamageProjectile);
        }
        
        public void OnFireRight(InputValue value)
        {
            SpawnProjectile(FlyweightObjectType.HealProjectile);
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