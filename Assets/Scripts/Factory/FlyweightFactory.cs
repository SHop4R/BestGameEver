using System;
using System.Collections.Generic;
using BestGameEver.Core;
using BestGameEver.FlyweightObjects.Base;
using BestGameEver.FlyweightObjects.Flyweights;
using UnityEngine;
using UnityEngine.Pool;

namespace BestGameEver.Factory
{
    public class FlyweightFactory : MonoSingleton<FlyweightFactory>
    {
        [SerializeField] private FlyweightSo[] flyweightObjects;
        [SerializeField] private int defaultPoolSize;

        private readonly Dictionary<FlyweightObjectType, FlyweightPoolTuple> _flyweightPoolsDictionary = new();

        private void Start()
        {
            PreGenerateObjects(flyweightObjects);
            flyweightObjects = null;
        }
        
        public Flyweight Spawn(FlyweightObjectType type)
        {
            FlyweightPoolTuple tuple = _flyweightPoolsDictionary[type];
            return tuple.Pool.Get();
        }

        public void ReturnToPool(Flyweight flyweight)
        {
            IObjectPool<Flyweight> pool = GetPoolFor(flyweight.settings);
            pool.Release(flyweight);
        }
        
        private IObjectPool<Flyweight> GetPoolFor(FlyweightSo so)
        {
            if (_flyweightPoolsDictionary.TryGetValue(so.ObjectType, out FlyweightPoolTuple tuple)) return tuple.Pool;
            
            tuple = new(so, CreatePoolFor(so));
            _flyweightPoolsDictionary.Add(so.ObjectType, tuple);
            return tuple.Pool;
        }

        private IObjectPool<Flyweight> CreatePoolFor(FlyweightSo so)
        {
            return new ObjectPool<Flyweight>
            (
                so.CreateProjectile,
                FlyweightSo.OnGetProjectile,
                FlyweightSo.OnReleaseProjectile,
                FlyweightSo.DestroyProjectile,
                true,
                defaultPoolSize,
                10
            );

        }
        private void PreGenerateObjects(Span<FlyweightSo> flyweights)
        {
            foreach (FlyweightSo so in flyweights)
            {
                FlyweightPoolTuple tuple = new(so, CreatePoolFor(so));
                _flyweightPoolsDictionary.Add(so.ObjectType, tuple);
            
                for (int i = 0; i < defaultPoolSize; i++)
                {
                    tuple.Pool.Get();
                }
            }
        }
    }
}
