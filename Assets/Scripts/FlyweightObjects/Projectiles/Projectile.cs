using System;
using System.Collections;
using BestGameEver.Core;
using BestGameEver.Enemies;
using BestGameEver.Factory;
using BestGameEver.FlyweightObjects.Base;
using UnityEngine;

namespace BestGameEver.FlyweightObjects.Projectiles
{
    [RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody)), DisallowMultipleComponent]
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

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Enemy")) return;
            if (!other.gameObject.TryGetComponent(out Enemy enemy)) return;

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