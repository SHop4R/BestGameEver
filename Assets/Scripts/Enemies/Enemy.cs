using BestGameEver.Enemies.Base;
using BestGameEver.Enemies.EnemyStates;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace BestGameEver.Enemies
{
    [RequireComponent(typeof(NavMeshAgent)), DisallowMultipleComponent, SelectionBase]
    public sealed class Enemy : MonoBehaviour
    {
        internal NavMeshAgent Agent;
        internal EnemyState CurrentState;
        internal MeshCollider EnemyCollider;

        [Header("Enemy Settings")]
        [SerializeField] internal float health;
        [SerializeField] internal GameObject aliveObject;
        [SerializeField] internal GameObject injuredObject;
        
        [Header("Patrol Settings")]
        [SerializeField] internal float patrolRadius;

        [Header("Events")]
        [SerializeField] internal EnemyEvent enemyEvent;
        
        [Header("UI")]
        [SerializeField] private Slider healthBar;
        [SerializeField] private Vector3 aliveHealthBarOffset;
        [SerializeField] private Vector3 injuredHealthBarOffset;
        
        private Transform _playerTransform;
        private Transform _enemyTransform;
        private EnemyState _lastEnemyState;

        internal Mesh AliveMesh;
        internal Mesh InjuredMesh;
        
        internal Vector3 Destination;

        private void Awake()
        {
            _playerTransform = FindObjectOfType<CharacterController>().transform;
            Agent = GetComponent<NavMeshAgent>();
            EnemyCollider = GetComponent<MeshCollider>();
            AliveMesh = aliveObject.GetComponent<MeshFilter>().sharedMesh;
            InjuredMesh = injuredObject.GetComponent<MeshFilter>().sharedMesh;
            
            EnemyCollider.sharedMesh = AliveMesh;
            
            aliveObject.SetActive(true);
            injuredObject.SetActive(false);
            
            _enemyTransform = transform;
            
            CurrentState = StateHelper.GetEnemyState(StateOfEnemy.Alive);
            _lastEnemyState = CurrentState;
        }

        private void Start()
        {
            InvokeRepeating(nameof(Patrol), 0f, 3f);
        }
        
        private void LateUpdate()
        {
            healthBar.transform.LookAt(_playerTransform, Vector3.up);
        }

        public void TakeDamage(float damage)
        {
            CurrentState.TakeDamage(this, damage);
            HandleHealthBar();
        }
        
        public void Heal(float heal)
        {
            CurrentState.Heal(this, heal);
            HandleHealthBar();
        }

        private void Patrol()
        {
            CurrentState.Patrol(this, _enemyTransform.position);
        }

        private void HandleHealthBar()
        {
            healthBar.value = health;
            
            if (CurrentState == _lastEnemyState) return;
            
            healthBar.transform.localPosition = CurrentState switch
            {
                EnemyAliveState => aliveHealthBarOffset,
                EnemyInjuredState => injuredHealthBarOffset,
                _ => aliveHealthBarOffset
            };
            
            _lastEnemyState = CurrentState;
        }

        internal static Vector3 RandomNavMeshPosition(Vector3 position, float distance)
        {
            position += Random.insideUnitSphere * distance;
            
            NavMesh.SamplePosition(position, out NavMeshHit navHit, distance, NavMesh.AllAreas);
            return navHit.position;
        }
    }
}