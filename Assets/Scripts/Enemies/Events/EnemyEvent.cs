using BestGameEver.Enemies.Base;
using UnityEngine;
using UnityEngine.Events;

namespace BestGameEver.Enemies.Events
{
    [CreateAssetMenu(fileName = "new EnemyEvent", menuName = "Events/EnemyEvent", order = 1)]
    public sealed class EnemyEvent : ScriptableObject
    {
        private UnityEvent<EnemyBehaviour> _onEvent;
        
        public void RaiseEvent(EnemyBehaviour enemy)
        {
            _onEvent?.Invoke(enemy);
        }
        
        public void Subscribe(UnityAction<EnemyBehaviour> listener)
        {
            _onEvent.AddListener(listener);
        }
        
        public void Unsubscribe(UnityAction<EnemyBehaviour> listener)
        {
            _onEvent.RemoveListener(listener);
        }
    }
}