using System.Collections.Generic;
using UnityEngine;

namespace BestGameEver.Core
{
    public static class WaitForSecondsStorage
    {
        private static readonly Dictionary<float, WaitForSeconds> Storage = new();
        
        public static WaitForSeconds GenerateWaitForSeconds(float seconds)
        {
            if (Storage.TryGetValue(seconds, out WaitForSeconds waitForSeconds)) return waitForSeconds;
            
            waitForSeconds = new WaitForSeconds(seconds);
            Storage.Add(seconds, waitForSeconds);
            return waitForSeconds;
        }
    }
}