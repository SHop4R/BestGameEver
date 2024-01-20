using UnityEngine;
using UnityEngine.AI;

namespace BestGameEver.Enemies.Base
{
    public class EnemyStateMachine : EnemyBehaviour
    {
        [field: Header("Patrol Settings")]
        [field: SerializeField] internal float PatrolRadius{get; private set;}
        [SerializeField] private float patrolRepeatTime;
        
        
        internal IEnemyState CurrentState;
        internal Vector3 Destination;
        
        protected override void Awake()
        {
            base.Awake();
            CurrentState = StateHelper.GetEnemyState(StateOfEnemy.Alive);
        }
        
        protected virtual void Start()
        {
            float randomTime = Random.Range(0f, patrolRepeatTime);
            InvokeRepeating(nameof(Patrol), randomTime, patrolRepeatTime);
        }
        
        public virtual void TakeDamage(float damage)
        {
            CurrentState.TakeDamage(this, damage);
        }
        
        public virtual void Heal(float heal)
        {
            CurrentState.Heal(this, heal);
        }

        private void Patrol()
        {
            CurrentState.Patrol(this, EnemyTransform.position);
        }
        
        internal static void ChangeState(EnemyStateMachine enemy ,StateOfEnemy state)
        {
            enemy.CurrentState = StateHelper.GetEnemyState(state);
        }
        
        internal static Vector3 RandomNavMeshPosition(Vector3 position, float distance)
        {
            position += Random.insideUnitSphere * distance;
            
            NavMesh.SamplePosition(position, out NavMeshHit navHit, distance, NavMesh.AllAreas);
            return navHit.position;
        }
    }
}
