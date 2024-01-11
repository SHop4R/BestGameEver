using UnityEngine;

namespace BestGameEver.Core
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance => GetInstance();

        private static T GetInstance()
        {
            if (_instance != null) return _instance;

            T[] allInstances = FindObjectsOfType<T>();
            foreach (T obj in allInstances)
            {
                if (_instance == null)
                {
                    _instance = obj;
                    continue;
                }

                Destroy(obj.gameObject);
            }
        
            _instance ??= new GameObject(typeof(T).Name).AddComponent<T>();
            return _instance;
        }
    }
}