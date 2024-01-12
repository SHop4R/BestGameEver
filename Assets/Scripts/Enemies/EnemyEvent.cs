using UnityEngine;
using UnityEngine.Events;

namespace BestGameEver.Enemies
{
    [CreateAssetMenu(fileName = "new EnemyEvent", menuName = "Events/EnemyEvent", order = 1)]
    public sealed class EnemyEvent : ScriptableObject
    {
        private UnityEvent<Enemy> _onEvent;
        
        public void RaiseEvent(Enemy enemy)
        {
            _onEvent?.Invoke(enemy);
        }
        
        public void Subscribe(UnityAction<Enemy> listener)
        {
            _onEvent.AddListener(listener);
        }
        
        public void Unsubscribe(UnityAction<Enemy> listener)
        {
            _onEvent.RemoveListener(listener);
        }
    }
}