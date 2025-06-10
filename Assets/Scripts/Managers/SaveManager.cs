
using System.Collections.Generic;
using System.IO;
using Dev;
using Saving;
using UI;
using UnityEngine;
// ReSharper disable HeuristicUnreachableCode
#pragma warning disable CS0162 // Unreachable code detected


namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        public GameSaveData gameSaveData;

        public static SaveManager instance;
        
        private readonly HashSet<string> currentSessionIds = new(); 
        // ThisKeeps track of all IDs seen this session BEFORE It deletes objects that were saved as deleted
        // like roll call to check for any IDs that should not exist anymore (renamed or deleted).
        
        //And a HashSet is a collection of UNIQUE strings optimized for fast lookup.
        
        
        private string savePath;

        //Yo im looking for the method for when the game closes vv
        private void OnApplicationQuit()
        {
            if(DevConfig.SAVE_ON_EXIT) SaveGame();
        }
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
            
            savePath = Application.persistentDataPath + "/savefile.dat";
            LoadSave();
        }

        public void SaveGame()
        {            
            // Only keep flags that were used this session
            var cleanDict = new Dictionary<string, bool>();
            
            foreach (var id in currentSessionIds)
            {
                if (gameSaveData.SaveFlags.TryGetValue(id, out var flag)) cleanDict[id] = flag;
            }
            gameSaveData.SaveFlags = cleanDict;
            
            gameSaveData.PrepareForSave();
            string json = JsonUtility.ToJson(gameSaveData, true);
            File.WriteAllText(savePath, json);
            Debug.Log("Game saved to: " + savePath);
            GameUI.instance.SavedGame();
        }
        
        //references: https://docs.unity3d.com/2022.3/Documentation/Manual/script-Serialization.html
        private void LoadSave()
        {
            if (File.Exists(savePath) && !DevConfig.FORCE_NEW_SAVE)
            {
                string jsonSave = File.ReadAllText(savePath);
                gameSaveData = JsonUtility.FromJson<GameSaveData>(jsonSave);
                gameSaveData.LoadFromSavedData(); // Load the dictionary from saved data
                Debug.Log("Game loaded.");
            }
            else
            {
                gameSaveData = new GameSaveData(); // Create fresh save
                Debug.Log("No save file found. Creating new data.");
            }
        }
        public bool ShouldExist(string id)
        {
            currentSessionIds.Add(id);
            
           gameSaveData.SaveFlags.TryAdd(id, true); //will fail silently if the key already exists
            Debug.Log("should exist: " + id + " = " + gameSaveData.SaveFlags[id]);
            return gameSaveData.SaveFlags[id];
        }
        public void ChangeFlag(string id, bool value)
        {
            gameSaveData.SaveFlags[id] = value;
            Debug.Log("Flag changed: " + id + " = " + value);
            if(DevConfig.SAVE_ON_CHANGE) SaveGame();
        }
    }
}
