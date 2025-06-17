using DialogueFramework;
using Managers;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Levels.Level0
{
    public class DialogueEvents : MonoBehaviour
    {
        [SerializeField] private Transform fakeOtto;
        [SerializeField] private Transform fairyLocation;

        private void CustomEvent(string eventName)
        {
            switch (eventName)
            {
                case "Where am I":
                CameraManager.instance.LerpZoom(10);
                CameraManager.instance.LookAtLocation(new Vector3(0f, 6f, 0f));
                    break;
                case "Reset look":
                    CameraManager.instance.LerpZoom(15);
                    CameraManager.instance.LookAt(RuriMovement.instance.transform);
                    break;
                case "Look at fake":
                    CameraManager.instance.LookAt(fakeOtto, 3f);
                    break;
                case "Look at fairy":
                    CameraManager.instance.LookAt(fairyLocation);
                    break;
                case "SaveGame": 
                    SaveManager.instance.SaveGame();
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
