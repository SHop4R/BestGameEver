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

        private void Awake()
        {
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
            if (_flyweightPoolsDictionary.TryGetValue(so.type, out FlyweightPoolTuple tuple)) return tuple.Pool;

            FlyweightPoolTuple newTuple = new(so, CreatePoolFor(so));
            _flyweightPoolsDictionary.Add(so.type, newTuple);
            return newTuple.Pool;
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
