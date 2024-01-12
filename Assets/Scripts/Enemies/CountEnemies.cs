using System.Collections.Generic;
using UnityEngine;

namespace BestGameEver.Enemies
{
    public class CountEnemies : MonoBehaviour
    {
        [SerializeField] private EnemyEvent enemyEvent;
        private readonly List<Enemy> _enemies = new();

        [SerializeField] private GameObject winScreen;

        private void Awake()
        {
            winScreen.SetActive(false);
        }

        private void Start()
        {
            _enemies.AddRange(FindObjectsOfType<Enemy>());
        }

        private void OnEnable()
        {
            enemyEvent.Subscribe(OnEnemyDeath);
        }
        
        private void OnDisable()
        {
            enemyEvent.Unsubscribe(OnEnemyDeath);
        }
        
        private void OnEnemyDeath(Enemy enemy)
        {
            _enemies.Remove(enemy);
            
            if (_enemies.Count == 0)
            {
                winScreen.SetActive(true);
            }
        }
    }
}