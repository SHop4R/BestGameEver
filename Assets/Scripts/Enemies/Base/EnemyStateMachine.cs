using BestGameEver.Enemies.Events;
using UnityEngine;
using UnityEngine.AI;

namespace BestGameEver.Enemies.Base
{
    [RequireComponent(typeof(NavMeshAgent)), DisallowMultipleComponent, SelectionBase]
    public class EnemyStateMachine : MonoBehaviour
    {
        internal EnemyState CurrentState;
        internal NavMeshAgent Agent;
        internal MeshCollider EnemyCollider;
        
        [Header("Enemy Settings")]
        [SerializeField] internal float health;
        [SerializeField] internal GameObject aliveObject;
        [SerializeField] internal GameObject injuredObject;
        
        [Header("Patrol Settings")]
        [SerializeField] internal float patrolRadius;

        [Header("Events")]
        [SerializeField] internal EnemyEvent enemyEvent;
        
        internal Mesh AliveMesh;
        internal Mesh InjuredMesh;
        
        internal Vector3 Destination;

        private Transform _enemyTransform;
        
        protected virtual void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            EnemyCollider = GetComponent<MeshCollider>();
            
            AliveMesh = aliveObject.GetComponent<MeshFilter>().sharedMesh;
            aliveObject.SetActive(true);
            
            InjuredMesh = injuredObject.GetComponent<MeshFilter>().sharedMesh;
            injuredObject.SetActive(false);
            
            CurrentState = StateHelper.GetEnemyState(StateOfEnemy.Alive);
            
            EnemyCollider.sharedMesh = AliveMesh;
            
            _enemyTransform = transform;
        }
        
        protected virtual void Start()
        {
            InvokeRepeating(nameof(Patrol), 0f, 3f);
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
            CurrentState.Patrol(this, _enemyTransform.position);
        }
        
        internal static Vector3 RandomNavMeshPosition(Vector3 position, float distance)
        {
            position += Random.insideUnitSphere * distance;
            
            NavMesh.SamplePosition(position, out NavMeshHit navHit, distance, NavMesh.AllAreas);
            return navHit.position;
        }
    }
}
