using DialogueFramework;
using Managers;
using UnityEngine;

namespace Levels.Level0
{
    public class MonkeyEncounterBlocker : MonoBehaviour
    {
        private Collider2D _collider2D;
        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
            CutsceneManager.cutsceneStarted += CutsceneStarted;
            DialogueSystem.onDialogueStart += DialogueStarted;
        }

        private void CutsceneStarted(string cutsceneName)
        {
            if (!cutsceneName.Equals("Monkey Encounter")) return;
            _collider2D.enabled = true;
        }
        private void DialogueStarted(string dialogueName)
        {
            if (!dialogueName.Equals("Regret")) return;
            _collider2D.enabled = false;
        }
    
        private void OnDestroy()
        {
            CutsceneManager.cutsceneStarted -= CutsceneStarted;
            DialogueSystem.onDialogueStart -= DialogueStarted;
        }
    }
}
