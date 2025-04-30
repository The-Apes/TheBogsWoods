using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class PoorMonkey : MonoBehaviour
{
    public GameObject healthBar;
    private CowardEnemy _cowardEnemy;
    private bool doOnce = false;
    public DialogueAsset dialogue; // The dialogue to be triggered
    
    private void Start()
    {
        GameEvents.OnDialogueEnd += OnDialogueComplete;
        _cowardEnemy = GetComponent<CowardEnemy>();
    }
    private void OnCutsceneStopped(PlayableDirector director)
    {
        
    }
    private void OnDialogueComplete(string dialogue)
    {
        if (dialogue == "Encounter")
        {
            healthBar.SetActive(true);
        }
    }

    private void Update()
    {
        if (_cowardEnemy.health <= 0 && !doOnce)
        {
            doOnce = true;
            healthBar.SetActive(false);
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            DialogueManager.Instance.StartDialogue(dialogue.dialogue);
        }
    }
    
}
