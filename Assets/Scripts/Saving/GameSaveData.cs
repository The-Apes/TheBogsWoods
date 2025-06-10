using System;
using System.Collections.Generic;
using UnityEngine;

namespace Saving
{
    [System.Serializable]
    public class GameSaveData
    {
        public Vector3 playerPosition; 
        public int maxHp = 5; //in case we want to have upgrades in our game
        public int hp = 5;
        public bool hasWeapon = false;
        public bool hasFairy = false;
        public bool hasOtto = false;
    
        public List<string> keys = new List<string>();
        public List<bool> values = new List<bool>();
        
        [NonSerialized] // the JSON Utility doesn't support dictonaries, so we gotta convert dis one right heya
        public Dictionary<string, bool> SaveFlags = new Dictionary<string, bool>();

        public void PrepareForSave() //translates the dictionarty into 2 lists
        {
            keys.Clear();
            values.Clear();
            foreach (var entry in SaveFlags)
            {
                keys.Add(entry.Key);
                values.Add(entry.Value);
            }   
        }
        
        public void LoadFromSavedData()
        {
            SaveFlags = new Dictionary<string, bool>();
            for (int i = 0; i < Mathf.Min(keys.Count, values.Count); i++)
            {
                SaveFlags[keys[i]] = values[i];
            }
        }
        
    
    }
}
