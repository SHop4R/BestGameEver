using System.Collections.Generic;
using UnityEngine;

namespace BestGameEver.FlyweightObjects.Base
{
    internal static class WaitForSecondsHelper
    {
        private static readonly Dictionary<float, WaitForSeconds> Storage = new();
        
        internal static WaitForSeconds GetWaitForSeconds(float seconds)
        {
            if (Storage.TryGetValue(seconds, out WaitForSeconds waitForSeconds)) return waitForSeconds;
            
            waitForSeconds = new(seconds);
            Storage.Add(seconds, waitForSeconds);
            return waitForSeconds;
        }
    }
}