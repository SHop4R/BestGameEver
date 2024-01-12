using UnityEngine;

namespace BestGameEver.Enemies
{
    public abstract class BaseEnemyState
    {
        public abstract void TakeDamage(Enemy enemy, float damage);
        public abstract void Heal(Enemy enemy, float heal);
        public abstract void Patrol(Enemy enemy, Vector3 position);

        protected static void ChangeState(Enemy enemy, BaseEnemyState state)
        {
            enemy.CurrentState = state;
        }
    }
}