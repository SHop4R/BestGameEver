using UnityEngine;

namespace BestGameEver.Core
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance{ get; private set; }

        protected virtual void Awake()
        {
            if (Instance is not null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this as T;
        }
    }
}