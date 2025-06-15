using System;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Saving
{
    public class SaveableObject : MonoBehaviour
    {
        public string saveID;
        [SerializeField] private bool falseOnDestroy;
        [NonSerialized] public bool selfDestroy;

        private void Start()
        {
            if (!SaveManager.instance) return;
            if (!SaveManager.instance.ShouldExist(saveID))
            {
                selfDestroy = true;
                Destroy(gameObject);
            }
        }
        private void OnDestroy()
        {
            if (selfDestroy) return;
            if(SceneChangeManager.isSceneChanging)return;
            if (!SaveManager.instance) return;
            if (falseOnDestroy) SaveManager.instance.ChangeFlag(saveID, false);
            
        }
    }
}
