using BestGameEver.Enemies.Base;
using UnityEngine;

namespace BestGameEver.Enemies.EnemyStates
{
    internal sealed class EnemyInjuredState : IEnemyState
    {
        public void TakeDamage(EnemyStateMachine enemy, float damage)
        {
            enemy.enemyEvent.RaiseEvent(enemy);
            Object.Destroy(enemy.gameObject);
        }

        public void Heal(EnemyStateMachine enemy, float heal)
        {
            Revive(enemy);
        }

        public void Patrol(EnemyStateMachine enemy, Vector3 position) {}
        
        private static void Revive(EnemyStateMachine enemy)
        {
            enemy.InjuredObject.SetActive(false);
            enemy.AliveObject.SetActive(true);
            
            enemy.Agent.isStopped = false;
            enemy.health = 10;
            
            EnemyStateMachine.ChangeState(enemy, StateOfEnemy.Alive);

            enemy.EnemyCollider.sharedMesh = enemy.AliveMesh;
        }
    }
}