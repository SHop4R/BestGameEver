using BestGameEver.Enemies.Events;
using UnityEngine;
using UnityEngine.AI;

namespace BestGameEver.Enemies.Base
{
    [RequireComponent(typeof(NavMeshAgent)), DisallowMultipleComponent, SelectionBase]
    public class EnemyBehaviour : MonoBehaviour
    {
        internal NavMeshAgent Agent{get; private set;}
        internal MeshCollider EnemyCollider{get; private set;}
        
        [Header("Enemy Settings")]
        [SerializeField] internal float health;
        [field: SerializeField] internal GameObject AliveObject{get; private set;}
        [field: SerializeField] internal GameObject InjuredObject{get; private set;}
        

        [Header("Events")]
        [SerializeField] internal EnemyEvent enemyEvent;
        
        internal Mesh AliveMesh{get; private set;}
        internal Mesh InjuredMesh{get; private set;}
        
        protected Transform EnemyTransform;

        protected virtual void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            EnemyCollider = GetComponent<MeshCollider>();
            
            AliveMesh = AliveObject.GetComponent<MeshFilter>().sharedMesh;
            AliveObject.SetActive(true);
            
            InjuredMesh = InjuredObject.GetComponent<MeshFilter>().sharedMesh;
            InjuredObject.SetActive(false);
            
            EnemyCollider.sharedMesh = AliveMesh;
            
            EnemyTransform = transform;
        }
    }
}