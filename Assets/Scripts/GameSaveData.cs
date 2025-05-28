using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameSaveData
{
    public int hp = 5;
    public bool hasWeapon = false;
    public bool hasFairy = false;
    public Dictionary<string, bool> CutscenesCompleted = new Dictionary<string, bool>();
    public Dictionary<string, bool> EnemiesKilled = new Dictionary<string, bool>();
}
