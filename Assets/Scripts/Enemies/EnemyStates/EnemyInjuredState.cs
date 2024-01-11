using UnityEngine;

namespace BestGameEver.Enemies.EnemyStates
{
    public class EnemyInjuredState : EnemyState
    {
        public override void TakeDamage(Enemy enemy, float damage)
        {
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
        }
    }
}