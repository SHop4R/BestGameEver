using BestGameEver.Enemies.Base;
using UnityEngine;

namespace BestGameEver.Enemies.EnemyStates
{
    internal sealed class EnemyInjuredState : EnemyState
    {
        public override void TakeDamage(EnemyStateMachine enemy, float damage)
        {
            enemy.enemyEvent.RaiseEvent(enemy);
            Object.Destroy(enemy.gameObject);
        }

        public override void Heal(EnemyStateMachine enemy, float heal)
        {
            Revive(enemy);
        }

        public override void Patrol(EnemyStateMachine enemy, Vector3 position) {}
        
        private static void Revive(EnemyStateMachine enemy)
        {
            enemy.injuredObject.SetActive(false);
            enemy.aliveObject.SetActive(true);
            
            enemy.Agent.isStopped = false;
            enemy.health = 10;
            
            ChangeState(enemy, StateOfEnemy.Alive);

            enemy.EnemyCollider.sharedMesh = enemy.AliveMesh;
        }
    }
}