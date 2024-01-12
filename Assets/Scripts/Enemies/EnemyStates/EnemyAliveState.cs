using UnityEngine;

namespace BestGameEver.Enemies.EnemyStates
{
    public class EnemyAliveState : BaseEnemyState
    {
        private Vector3 _destination;
        
        public override void TakeDamage(Enemy enemy, float damage)
        {
            enemy.health -= damage;
            
            if (enemy.health <= 0)
            {
                Injure(enemy);
            }
        }

        public override void Heal(Enemy enemy, float heal)
        {
            if (enemy.health >= 100) return;
            
            enemy.health += heal;
            enemy.health = Mathf.Clamp(enemy.health, 0, 100);
        }

        public override void Patrol(Enemy enemy, Vector3 position)
        {
            if (enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
            {
                _destination = Enemy.RandomNavMeshPosition(position, enemy.patrolRadius);
            }
            
            enemy.Agent.SetDestination(_destination);
        }

        private static void Injure(Enemy enemy)
        {
            enemy.aliveObject.SetActive(false);
            enemy.injuredObject.SetActive(true);
            
            enemy.Agent.isStopped = true;
            
            ChangeState(enemy, new EnemyInjuredState());

            enemy.EnemyCollider.sharedMesh = enemy.InjuredMesh;
        }
    }
}