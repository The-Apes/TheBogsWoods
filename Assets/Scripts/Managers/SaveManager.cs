
using System;
using System.Collections.Generic;
using System.IO;
using Dev;
using Player;
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
        
        public GameObject potionPrefab; 

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
            GetRuriInfo();
            GetSparePotions();
            
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
                LoadRuriInfo();
                LoadSparePotions();
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
        private void GetRuriInfo()
        {
            var ruri = RuriMovement.instance;
            gameSaveData.playerPosition = ruri.transform.position;
            
            gameSaveData.hp = FindFirstObjectByType<PlayerHealth>().currentHealth;
            gameSaveData.maxHp = FindFirstObjectByType<PlayerHealth>().maxHealth;

            gameSaveData.hasWeapon = ruri.hasWeapon;
            gameSaveData.hasFairy = ruri.hasFairy;
            gameSaveData.hasOtto = ruri.hasOtto;

        }
        private void LoadRuriInfo()
        {
            var ruri = FindFirstObjectByType<RuriMovement>();
            ruri.transform.position = gameSaveData.playerPosition;
            
            var playerHealth = FindFirstObjectByType<PlayerHealth>();
            playerHealth.currentHealth = gameSaveData.hp;
            playerHealth.maxHealth = gameSaveData.maxHp;
            playerHealth.healthUI.SetMaxHearts(gameSaveData.maxHp);
            playerHealth.healthUI.UpdateHearts(gameSaveData.hp);

            ruri.hasWeapon = gameSaveData.hasWeapon;
            ruri.hasFairy = gameSaveData.hasFairy;
            ruri.hasOtto = gameSaveData.hasOtto;
        }
        private void GetSparePotions()
        {
            gameSaveData.sparePotions.Clear();
            var sparePotions = FindObjectsByType<HealthPotion>(FindObjectsSortMode.None);
            foreach (var potion in sparePotions)
            {
                gameSaveData.sparePotions.Add(potion.transform.position);
            }
        }

        private void LoadSparePotions()
        {
            foreach (var position in gameSaveData.sparePotions)
            {
                Instantiate(potionPrefab, position, Quaternion.identity);
            }

        }
        
    }
}
