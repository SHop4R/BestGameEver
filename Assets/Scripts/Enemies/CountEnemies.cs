﻿using System.Collections.Generic;
using BestGameEver.Core;
using BestGameEver.Enemies.Base;
using BestGameEver.Enemies.Events;
using UnityEngine;

namespace BestGameEver.Enemies
{
    internal sealed class CountEnemies : MonoSingleton<CountEnemies>
    {
        [SerializeField] private EnemyEvent enemyEvent;
        private readonly List<EnemyBehaviour> _enemies = new();

        [SerializeField] private GameObject winScreen;

        protected override void Awake()
        {
            winScreen.SetActive(false);
        }

        private void Start()
        {
            _enemies.AddRange(FindObjectsOfType<EnemyBehaviour>());
        }

        private void OnEnable()
        {
            enemyEvent.Subscribe(OnEnemyDeath);
        }
        
        private void OnDisable()
        {
            enemyEvent.Unsubscribe(OnEnemyDeath);
        }
        
        private void OnEnemyDeath(EnemyBehaviour enemy)
        {
            if (enemy == null) return;
            
            _enemies.Remove(enemy);
            
            if (_enemies.Count == 0)
            {
                winScreen.SetActive(true);
            }
        }
    }
}