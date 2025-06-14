using System;
using DialogueFramework;
using Managers;
using UnityEngine;

public class VersionVincent : MonoBehaviour, IInteractable
{
    private bool talkedToOnce;
    [SerializeField] private DialogueAsset introDialogue;
    [SerializeField] private DialogueAsset changes;

    private void Start()
    {
        DialogueSystem.onDialogueNextLine += CustomEvent;
    }

    public void Interact()
    {
        DialogueManager.instance.StartDialogue(talkedToOnce ? changes : introDialogue);
        talkedToOnce = true;
    }

    public bool CanInteract()
    {
        return true;
    }

    private void CustomEvent(string eventName)
    {
        if (eventName == "GET OUT") RuriMovement.instance.ApplyForceInDirection(direction:RuriMovement.Direction.Down, 10f);
    }
}