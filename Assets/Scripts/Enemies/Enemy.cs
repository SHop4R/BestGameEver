using System;
using BestGameEver.Enemies.EnemyStates;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace BestGameEver.Enemies
{
    [RequireComponent(typeof(NavMeshAgent)), DisallowMultipleComponent]
    public sealed class Enemy : MonoBehaviour
    {
        internal NavMeshAgent Agent;
        internal EnemyState CurrentState;

        [Header("Enemy Settings")]
        [SerializeField] internal float health;
        [SerializeField] internal GameObject aliveObject;
        [SerializeField] internal GameObject injuredObject;
        
        [Header("Patrol Settings")]
        [SerializeField] internal float patrolRadius;
        
        private Transform _enemyTransform;

        private void Awake()
        {
            CurrentState = new EnemyAliveState();
            Agent = GetComponent<NavMeshAgent>();
            _enemyTransform = transform;
            
            aliveObject.SetActive(true);
            injuredObject.SetActive(false);
        }

        private void Start()
        {
            InvokeRepeating(nameof(Patrol), 0f, 0.5f);
        }

        internal static Vector3 RandomNavMeshPosition(Vector3 position, float distance)
        {
            Vector3 randomDirection = Random.insideUnitSphere * distance;
            randomDirection += position;
            
            NavMesh.SamplePosition(randomDirection, out NavMeshHit navHit, distance, NavMesh.AllAreas);
            return navHit.position;
        }

        public void TakeDamage(float damage)
        {
            CurrentState.TakeDamage(this, damage);
            Debug.Log($"Enemy took {damage} damage");
        }
        
        public void Heal(float heal)
        {
            CurrentState.Heal(this, heal);
        }

        private void Patrol()
        {
            CurrentState.Patrol(this, _enemyTransform.position);
        }
    }
}