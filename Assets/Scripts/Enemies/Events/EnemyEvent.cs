using BestGameEver.Enemies.Base;
using UnityEngine;
using UnityEngine.Events;

namespace BestGameEver.Enemies.Events
{
    [CreateAssetMenu(fileName = "new EnemyEvent", menuName = "Events/EnemyEvent", order = 1)]
    public sealed class EnemyEvent : ScriptableObject
    {
        private UnityEvent<EnemyStateMachine> _onEvent;
        
        public void RaiseEvent(EnemyStateMachine enemy)
        {
            _onEvent?.Invoke(enemy);
        }
        
        public void Subscribe(UnityAction<EnemyStateMachine> listener)
        {
            _onEvent.AddListener(listener);
        }
        
        public void Unsubscribe(UnityAction<EnemyStateMachine> listener)
        {
            _onEvent.RemoveListener(listener);
        }
    }
}