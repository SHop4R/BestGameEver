using System.Collections.Generic;
using BestGameEver.Enemies.EnemyStates;

namespace BestGameEver.Enemies.Base
{
    internal static class StateHelper
    {
        private static readonly Dictionary<StateOfEnemy, EnemyState> Storage = new();
        
        internal static EnemyState GetEnemyState(StateOfEnemy state)
        {
            if (Storage.TryGetValue(state, out EnemyState enemyState)) return enemyState;

            enemyState = state switch
            {
                StateOfEnemy.Alive => new EnemyAliveState(),
                StateOfEnemy.Injured => new EnemyInjuredState(),
                _ => null
            };

            Storage.Add(state, enemyState);
            return enemyState;
        }
    }
}
