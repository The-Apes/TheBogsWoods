using Managers;
using UnityEngine;

namespace Levels.Level0
{
    public class OttoFloorHat : MonoBehaviour
    {
    
        private void CustomEvent(string eventName)
        {
            if(!eventName.Equals("Destroy Hat")) return;
            DialogueSystem.onDialogueNextLine -= CustomEvent;
            Destroy(gameObject);
        }
        public void Start()
        {
            DialogueSystem.onDialogueNextLine += CustomEvent;
        }

        public void OnDisable()
        {
            DialogueSystem.onDialogueNextLine -= CustomEvent;
        }
    }
}
