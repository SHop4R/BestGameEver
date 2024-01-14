using UnityEngine;

namespace BestGameEver.Enemies.Base
{
    internal abstract class EnemyState
    {
        public abstract void TakeDamage(Enemy enemy, float damage);
        public abstract void Heal(Enemy enemy, float heal);
        public abstract void Patrol(Enemy enemy, Vector3 position);
        
        protected static void ChangeState(Enemy enemy, StateOfEnemy state)
        {
            EnemyState newState = StateHelper.GetEnemyState(state);
            enemy.CurrentState = newState;
        }
    }
}