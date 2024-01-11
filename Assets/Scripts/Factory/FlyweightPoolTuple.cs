using BestGameEver.FlyweightObjects.Base;
using UnityEngine.Pool;

namespace BestGameEver.Factory
{
    internal struct FlyweightPoolTuple
    {
        public readonly FlyweightSo So;
        public readonly IObjectPool<Flyweight> Pool;

        public FlyweightPoolTuple(FlyweightSo so, IObjectPool<Flyweight> pool)
        {
            So = so;
            Pool = pool;
        }
    }
}
