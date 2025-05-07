using Managers;
using UnityEngine;
using UnityEngine.Playables;

namespace Levels.Level0
{
    public class PoorMonkey : MonoBehaviour
    {
        public GameObject healthBar;
        private CowardEnemy _cowardEnemy;
        private bool doOnce = false;
        public DialogueAsset dialogue; // The dialogue to be triggered
    
        private void Start()
        {
            GameEvents.onDialogueEnd += OnDialogueComplete;
            _cowardEnemy = GetComponent<CowardEnemy>();
        }
        private void OnCutsceneStopped(PlayableDirector director)
        {
        
        }
        private void OnDialogueComplete(string instigatingDialogue)
        {
            if (instigatingDialogue == "Encounter")
            {
                healthBar.SetActive(true);
            }
        }

        private void Update()
        {
            if (!(_cowardEnemy.health <= 0) || doOnce) return;
            doOnce = true;
            healthBar.SetActive(false);
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            DialogueManager.instance.StartDialogue(dialogue.dialogue);
        }
    
    }
}
