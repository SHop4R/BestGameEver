using BestGameEver.FlyweightObjects.Flyweights;
using UnityEngine;

namespace BestGameEver.FlyweightObjects.Projectiles
{
    [CreateAssetMenu(fileName = "new Projectile", menuName = "Scriptable Objects/Projectile", order = 0)]
    public sealed class ProjectileSo : FlyweightSo
    {
        [field: SerializeField] public float Speed{ get; private set; }
        [field: SerializeField] public int EffectAmount{ get; private set; }
        [field: SerializeField] public float Lifetime{ get; private set; }
    }
}