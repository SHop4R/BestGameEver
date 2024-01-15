using System;
using System.Collections;
using BestGameEver.Enemies;
using BestGameEver.Enemies.Base;
using BestGameEver.Factory;
using BestGameEver.FlyweightObjects.Base;
using BestGameEver.FlyweightObjects.Flyweights;
using UnityEngine;

namespace BestGameEver.FlyweightObjects.Projectiles
{
    [DisallowMultipleComponent]
    public sealed class Projectile : Flyweight
    {
        private ProjectileSo Settings => (ProjectileSo)settings;
        
        [SerializeField] private LayerMask layers;
        
        private float _radius;
        private readonly Collider[] _results = { null };
        
        private void Awake()
        {
            _radius = transform.localScale.x / 2;
        }

        private void OnEnable()
        {
            StartCoroutine(Lifetime());
        }
        
        private void Update()
        {
            transform.Translate(Vector3.forward * (Settings.Speed * Time.deltaTime));
            CheckForCollision();
        }
        
        private void OnDisable()
        {
            StopCoroutine(Lifetime());
        }

        private IEnumerator Lifetime()
        {
            yield return WaitForSecondsHelper.GetWaitForSeconds(Settings.Lifetime);
            FlyweightFactory.Instance.ReturnToPool(this);
        }
        
        private void CheckForCollision()
        {
            int numberOfCollidersChecked = Physics.OverlapSphereNonAlloc(transform.position, _radius, _results, layers);

            if (numberOfCollidersChecked == 0) return;

            switch (_results[0].tag)
            {
                case "Player":
                    break;
                
                case "Enemy":
                    if (!_results[0].TryGetComponent(out EnemyUI enemy)) return;
                    Affect(enemy);
                    break;
                
                default:
                    FlyweightFactory.Instance.ReturnToPool(this);
                    break;
            }
        }
        
        private void Affect(EnemyStateMachine enemy)
        {
            FlyweightFactory.Instance.ReturnToPool(this);
            
            switch (Settings.ObjectType)
            {
                case FlyweightObjectType.DamageProjectile:
                    enemy.TakeDamage(Settings.EffectAmount);
                    break;

                case FlyweightObjectType.HealProjectile:
                    enemy.Heal(Settings.EffectAmount);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}