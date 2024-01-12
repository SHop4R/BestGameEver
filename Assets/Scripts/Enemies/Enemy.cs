using BestGameEver.Enemies.EnemyStates;
using UnityEngine;
using UnityEngine.AI;

namespace BestGameEver.Enemies
{
    [RequireComponent(typeof(NavMeshAgent)), DisallowMultipleComponent]
    public sealed class Enemy : MonoBehaviour
    {
        internal NavMeshAgent Agent;
        internal BaseEnemyState CurrentState;
        internal MeshCollider EnemyCollider;

        [Header("Enemy Settings")]
        [SerializeField] internal float health;
        [SerializeField] internal GameObject aliveObject;
        [SerializeField] internal GameObject injuredObject;
        
        [Header("Patrol Settings")]
        [SerializeField] internal float patrolRadius;

        [Header("Events")]
        [SerializeField] internal EnemyEvent enemyEvent;
        
        private Transform _enemyTransform;

        internal Mesh AliveMesh;
        internal Mesh InjuredMesh;

        private void Awake()
        {
            CurrentState = new EnemyAliveState();
            EnemyCollider = GetComponent<MeshCollider>();
            Agent = GetComponent<NavMeshAgent>();
            _enemyTransform = transform;
            
            aliveObject.SetActive(true);
            injuredObject.SetActive(false);
            
            AliveMesh = aliveObject.GetComponent<MeshFilter>().sharedMesh;
            InjuredMesh = injuredObject.GetComponent<MeshFilter>().sharedMesh;
            
            EnemyCollider.sharedMesh = AliveMesh;
        }

        private void Start()
        {
            InvokeRepeating(nameof(Patrol), 0f, 3f);
        }

        public void TakeDamage(float damage)
        {
            CurrentState.TakeDamage(this, damage);
        }
        
        public void Heal(float heal)
        {
            CurrentState.Heal(this, heal);
        }

        private void Patrol()
        {
            CurrentState.Patrol(this, _enemyTransform.position);
        }

        internal static Vector3 RandomNavMeshPosition(Vector3 position, float distance)
        {
            Vector3 randomDirection = Random.insideUnitCircle * distance;
            randomDirection += position;
            
            NavMesh.SamplePosition(randomDirection, out NavMeshHit navHit, 10f, NavMesh.AllAreas);
            return navHit.position;
        }
    }
}