using UnityEngine;

namespace BestGameEver.Enemies.EnemyStates
{
    public class EnemyInjuredState : BaseEnemyState
    {
        public override void TakeDamage(Enemy enemy, float damage)
        {
            enemy.enemyEvent.RaiseEvent(enemy);
            Object.Destroy(enemy.gameObject);
        }

        public override void Heal(Enemy enemy, float heal)
        {
            Revive(enemy);
        }

        public override void Patrol(Enemy enemy, Vector3 position)
        {
        }
        
        private static void Revive(Enemy enemy)
        {
            enemy.injuredObject.SetActive(false);
            enemy.aliveObject.SetActive(true);
            
            enemy.Agent.isStopped = false;
            enemy.health = 10;
            
            ChangeState(enemy, new EnemyAliveState());

            enemy.EnemyCollider.sharedMesh = enemy.AliveMesh;
        }
    }
}