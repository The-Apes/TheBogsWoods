
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Saving;
using UI;
using UnityEngine;

namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        public GameSaveData gameSaveData;

        public static SaveManager instance;
        
        //private const bool DEV_SKIP_SAVE = true; // Set to true if you want to save changes made in the game
        private const bool DEV_FORCE_NEW_SAVE = false; // every time game starts, fresh save won't load
        private const bool DEV_SAVE_ON_CHANGE = true; 
        
        private HashSet<string> currentSessionIds = new(); 
        // ThisKeeps track of all IDs seen this session BEFORE It deletes objects that were saved as deleted
        // like roll call to check for any IDs that should not exist anymore (renamed or deleted).
        
        //And a HashSet is a collection of UNIQUE strings optimized for fast lookup.
        
        
        private string savePath;

        
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
                if (gameSaveData.SaveFlags.ContainsKey(id))
                    cleanDict[id] = gameSaveData.SaveFlags[id];
            }
            gameSaveData.SaveFlags = cleanDict;
            
            gameSaveData.PrepareForSave();
            string json = JsonUtility.ToJson(gameSaveData, true);
            File.WriteAllText(savePath, json);
            Debug.Log("Game saved to: " + savePath);
            GameUI.instance.SavedGame();
        }
        
        //refereces: https://docs.unity3d.com/2022.3/Documentation/Manual/script-Serialization.html
        public void LoadSave()
        {
            if (File.Exists(savePath) && !DEV_FORCE_NEW_SAVE)
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
            if (gameSaveData.SaveFlags.ContainsKey(id))
            {
                gameSaveData.SaveFlags[id] = value;
            }
            else
            {
                gameSaveData.SaveFlags.Add(id, value);
            }
            if(DEV_SAVE_ON_CHANGE) SaveGame();
        }
    }
}
