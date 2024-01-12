using System.Collections.Generic;
using BestGameEver.Core;
using BestGameEver.FlyweightObjects.Base;
using UnityEngine;
using UnityEngine.Pool;

namespace BestGameEver.Factory
{
    public class FlyweightFactory : MonoSingleton<FlyweightFactory>
    {
        [SerializeField] private FlyweightSo[] flyweightObjects = { };
        
        private readonly Dictionary<FlyweightObjectType, FlyweightPoolTuple> _flyweightPoolsDictionary = new();

        protected override void Awake()
        {
            base.Awake();
            
            foreach (FlyweightSo so in flyweightObjects)
            {
                FlyweightPoolTuple tuple = new(so, CreatePoolFor(so));
                _flyweightPoolsDictionary.Add(so.type, tuple);
            }
            
            flyweightObjects = null;
        }

        public Flyweight Spawn(FlyweightObjectType type)
        {
            FlyweightPoolTuple tuple = _flyweightPoolsDictionary.GetValueOrDefault(type);
            return tuple.Pool.Get();
        }

        public void ReturnToPool(Flyweight flyweight)
        {
            IObjectPool<Flyweight> pool = GetPoolFor(flyweight.settings);
            pool.Release(flyweight);
        }
        
        private IObjectPool<Flyweight> GetPoolFor(FlyweightSo so)
        {
            if (_flyweightPoolsDictionary.TryGetValue(so.type, out FlyweightPoolTuple existingTuple)) return existingTuple.Pool;

            FlyweightPoolTuple createdTuple = new(so, CreatePoolFor(so));
            _flyweightPoolsDictionary.Add(so.type, createdTuple);
            return createdTuple.Pool;
        }

        private static IObjectPool<Flyweight> CreatePoolFor(FlyweightSo so)
        {
            return new ObjectPool<Flyweight>
            (
                so.CreateProjectile,
                so.OnGetProjectile,
                so.OnReleaseProjectile,
                so.DestroyProjectile,
                true,
                10,
                100
            );
        }
    }
}
