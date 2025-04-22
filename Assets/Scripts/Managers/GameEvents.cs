using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static Action<string> OnAreaChange;
    public static Action<string> OnDialogueEnd;

    public static void AreaChanged(string AreaName) => OnAreaChange?.Invoke(AreaName);
    public static void DialogueEnded(string dialogueName) => OnDialogueEnd?.Invoke(dialogueName);
    
}
