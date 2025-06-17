using System;
using AI;
using DialogueFramework;
using Enemies;
using Managers;
using UnityEngine;
using UnityEngine.Playables;

namespace Levels.Level0
{
    public class PoorMonkey : MonoBehaviour
    {
        public GameObject healthBar;
        private BaseAI _cowardEnemy;
        private bool doOnce = false;
        public DialogueAsset dialogue; // The dialogue to be triggered
    
        private void Start()
        {
            DialogueSystem.onDialogueEnd += OnDialogueComplete;
            _cowardEnemy = GetComponent<BaseAI>();
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
            if ((_cowardEnemy.currHealth != 1) || doOnce) return;
            doOnce = true;
            healthBar.SetActive(false);
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            print("print");
            DialogueManager.instance.StartDialogue(dialogue);
            SaveManager.instance.ChangeFlag("FirstMonkey", false);
        }

        private void OnDestroy()
        {
            DialogueSystem.onDialogueEnd -= OnDialogueComplete;

        }
    }
}
