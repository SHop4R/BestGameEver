using UnityEngine;

namespace BestGameEver.Enemies.Base
{
    internal abstract class EnemyState
    {
        public abstract void TakeDamage(EnemyStateMachine enemy, float damage);
        public abstract void Heal(EnemyStateMachine enemy, float heal);
        public abstract void Patrol(EnemyStateMachine enemy, Vector3 position);
        
        protected static void ChangeState(EnemyStateMachine enemy, StateOfEnemy state)
        {
            EnemyState newState = StateHelper.GetEnemyState(state);
            enemy.CurrentState = newState;
        }
    }
}