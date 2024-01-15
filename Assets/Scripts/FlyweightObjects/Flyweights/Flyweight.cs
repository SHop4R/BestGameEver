using UnityEngine;

namespace BestGameEver.FlyweightObjects.Flyweights
{
    [DisallowMultipleComponent]
    public abstract class Flyweight : MonoBehaviour
    {
        public FlyweightSo settings;
    }
}