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

        private void Start()
        {
            if (!SaveManager.instance) return;
            if(!SaveManager.instance.ShouldExist(saveID)) Destroy(gameObject);
        }
        private void OnDestroy()
        {
            if (!SaveManager.instance) return;
            if (falseOnDestroy) SaveManager.instance.ChangeFlag(saveID, false);
            
        }
    }
}
