using UnityEngine;

namespace BestGameEver.FlyweightObjects.Base
{
    [DisallowMultipleComponent]
    public abstract class Flyweight : MonoBehaviour
    {
        public FlyweightSo settings;
    }
}