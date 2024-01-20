using BestGameEver.Enemies.Base;
using UnityEngine;

namespace BestGameEver.Enemies.EnemyStates
{
    internal sealed class EnemyAliveState : IEnemyState
    {
        public void TakeDamage(EnemyStateMachine enemy, float damage)
        {
            enemy.health -= damage;
            
            if (enemy.health <= 0)
            {
                Injure(enemy);
            }
        }

        public void Heal(EnemyStateMachine enemy, float heal)
        {
            if (enemy.health >= 100) return;
            
            enemy.health += heal;
            enemy.health = Mathf.Clamp(enemy.health, 0, 100);
        }

        public void Patrol(EnemyStateMachine enemy, Vector3 position)
        {
            if (enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
            {
                enemy.Destination = EnemyStateMachine.RandomNavMeshPosition(position, enemy.PatrolRadius);
            }
            
            enemy.Agent.SetDestination(enemy.Destination);
        }

        private static void Injure(EnemyStateMachine enemy)
        {
            enemy.AliveObject.SetActive(false);
            enemy.InjuredObject.SetActive(true);
            
            enemy.Agent.isStopped = true;
            
            EnemyStateMachine.ChangeState(enemy, StateOfEnemy.Injured);

            enemy.EnemyCollider.sharedMesh = enemy.InjuredMesh;
        }
    }
}