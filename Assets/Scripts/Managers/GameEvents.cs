using System;
using UnityEngine;

namespace Managers
{
    public class GameEvents : MonoBehaviour
    {
        public static Action<string> onAreaChange;

        public static void AreaChanged(string areaName) => onAreaChange?.Invoke(areaName);
        
    
    }
}
