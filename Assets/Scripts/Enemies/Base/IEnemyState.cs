using UnityEngine;

namespace BestGameEver.Enemies.Base
{
    public interface IEnemyState
    {
        void TakeDamage(EnemyStateMachine enemy, float damage);
        void Heal(EnemyStateMachine enemy, float heal);
        void Patrol(EnemyStateMachine enemy, Vector3 position);
    }
}