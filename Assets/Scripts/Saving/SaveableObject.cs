using System;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Saving
{
    public class SaveableObject : MonoBehaviour
    {
        public string saveID;

        private void Start()
        {
            if (!SaveManager.instance) return;
            if(!SaveManager.instance.ShouldExist(saveID)) Destroy(gameObject);
        }
    }
}
