using System;
using UnityEngine;

namespace Managers
{
    public class GameEvents : MonoBehaviour
    {
        public static Action<string> onAreaChange;
        public static Action<string> onDialogueEnd;

        public static void AreaChanged(string areaName) => onAreaChange?.Invoke(areaName);
        public static void DialogueEnded(string dialogueName) => onDialogueEnd?.Invoke(dialogueName);
    
    }
}
