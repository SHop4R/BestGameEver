using BestGameEver.Enemies.Base;
using BestGameEver.Enemies.EnemyStates;
using UnityEngine;
using UnityEngine.UI;

namespace BestGameEver.Enemies
{
    public sealed class EnemyUI : EnemyStateMachine
    {
        private Transform _playerTransform;
        
        [Header("UI")]
        [SerializeField] private Slider healthBar;
        [SerializeField] private Vector3 aliveHealthBarOffset;
        [SerializeField] private Vector3 injuredHealthBarOffset;
        
        private EnemyState _lastEnemyState;

        protected override void Awake()
        {
            base.Awake();
            _playerTransform = FindObjectOfType<CharacterController>().transform;
            
            _lastEnemyState = CurrentState;
        }

        private void LateUpdate()
        {
            healthBar.transform.LookAt(_playerTransform, Vector3.up);
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            HandleHealthBar();
        }

        public override void Heal(float heal)
        {
            base.Heal(heal);
            HandleHealthBar();
        }

        private void HandleHealthBar()
        {
            healthBar.value = health;
            
            if (CurrentState == _lastEnemyState) return;
            
            healthBar.transform.localPosition = CurrentState switch
            {
                EnemyAliveState => aliveHealthBarOffset,
                EnemyInjuredState => injuredHealthBarOffset,
                _ => aliveHealthBarOffset
            };
            
            _lastEnemyState = CurrentState;
        }
    }
}