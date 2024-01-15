using BestGameEver.Factory;
using BestGameEver.FlyweightObjects.Base;
using BestGameEver.FlyweightObjects.Flyweights;
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
            Shoot(FlyweightObjectType.DamageProjectile);
        }
        
        public void OnFireRight(InputValue value)
        {
            Shoot(FlyweightObjectType.HealProjectile);
        }
        
        private void Shoot(FlyweightObjectType type)
        {
            if (Time.time - _lastFireRate < fireRate) return;
            
            Flyweight obj = FlyweightFactory.Instance.Spawn(type);
            
            Transform objTransform = obj.transform;
            objTransform.position = cinemachineCameraTarget.position;
            objTransform.rotation = cinemachineCameraTarget.rotation;
            
            _lastFireRate = Time.time;
        }
    }
}