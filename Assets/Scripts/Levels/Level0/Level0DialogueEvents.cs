using DialogueFramework;
using Managers;
using UnityEngine;
using Util;

namespace Levels.Level0
{
    public class DialogueEvents : MonoBehaviour
    {
        private void CustomEvent(string eventName)
        {
            switch (eventName)
            {
                case "Where am I":
                CameraManager.instance.LerpZoom(15);
                CameraManager.instance.LookAtLocation(new Vector3(0f, 6f, 0f));
                    break;
                case "Reset look":
                    CameraManager.instance.LerpZoom(20);
                    CameraManager.instance.LookAt(RuriMovement.instance.transform);
                    break;
                case "Look left":
                    RuriMovement.instance.Look(RuriMovement.Direction.Left);
                    break;
                case "Look right":
                    RuriMovement.instance.Look(RuriMovement.Direction.Right);
                    break;
                case "Look up":
                    RuriMovement.instance.Look(RuriMovement.Direction.Up);
                    break;
                case "Look down":
                    RuriMovement.instance.Look(RuriMovement.Direction.Down);
                    break;
            }
        }
        public void Start()
        {
            DialogueSystem.onDialogueNextLine += CustomEvent;
        }

        public void OnDestroy()
        {
            DialogueSystem.onDialogueNextLine -= CustomEvent;
        }
    }
}
