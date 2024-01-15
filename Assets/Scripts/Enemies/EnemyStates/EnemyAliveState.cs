using BestGameEver.Enemies.Base;
using UnityEngine;

namespace BestGameEver.Enemies.EnemyStates
{
    internal sealed class EnemyAliveState : EnemyState
    {
        public override void TakeDamage(EnemyStateMachine enemy, float damage)
        {
            enemy.health -= damage;
            
            if (enemy.health <= 0)
            {
                Injure(enemy);
            }
        }

        public override void Heal(EnemyStateMachine enemy, float heal)
        {
            if (enemy.health >= 100) return;
            
            enemy.health += heal;
            enemy.health = Mathf.Clamp(enemy.health, 0, 100);
        }

        public override void Patrol(EnemyStateMachine enemy, Vector3 position)
        {
            if (enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
            {
                enemy.Destination = EnemyStateMachine.RandomNavMeshPosition(position, enemy.patrolRadius);
            }
            
            enemy.Agent.SetDestination(enemy.Destination);
        }

        private static void Injure(EnemyStateMachine enemy)
        {
            enemy.aliveObject.SetActive(false);
            enemy.injuredObject.SetActive(true);
            
            enemy.Agent.isStopped = true;
            
            ChangeState(enemy, StateOfEnemy.Injured);

            enemy.EnemyCollider.sharedMesh = enemy.InjuredMesh;
        }
    }
}