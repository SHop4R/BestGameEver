using System;
using System.Collections;
using BestGameEver.Core;
using BestGameEver.Enemies;
using BestGameEver.Factory;
using BestGameEver.FlyweightObjects.Base;
using UnityEngine;

namespace BestGameEver.FlyweightObjects.Projectiles
{
    [RequireComponent(typeof(Collider)), DisallowMultipleComponent]
    public sealed class Projectile : Flyweight
    {
        private ProjectileSo SettingsForProjectile => (ProjectileSo)settings;

        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnEnable()
        {
            StartCoroutine(Lifetime());
        }
        
        private void Update()
        {
            transform.Translate(Vector3.forward * (SettingsForProjectile.speed * Time.deltaTime));
        }
        
        private void OnDisable()
        {
            StopCoroutine(Lifetime());
        }

        private IEnumerator Lifetime()
        {
            yield return WaitForSecondsStorage.GenerateWaitForSeconds(SettingsForProjectile.lifetime);
            FlyweightFactory.Instance.ReturnToPool(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case "Player":
                    break;
                
                case "Enemy":
                    if (!other.TryGetComponent(out Enemy enemy)) return;
                    Affect(enemy);
                    break;
                
                default:
                    FlyweightFactory.Instance.ReturnToPool(this);
                    break;
            }
        }

        private void Affect(Enemy enemy)
        {
            FlyweightFactory.Instance.ReturnToPool(this);
            
            switch (SettingsForProjectile.type)
            {
                case FlyweightObjectType.DamageProjectile:
                    enemy.TakeDamage(SettingsForProjectile.effectAmount);
                    break;

                case FlyweightObjectType.HealProjectile:
                    enemy.Heal(SettingsForProjectile.effectAmount);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}