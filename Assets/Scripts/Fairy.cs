using DialogueFramework;
using Managers;
using Player;
using UnityEngine;

public class Fairy : MonoBehaviour, IInteractable
{
    private bool talkedToOnce;
    private bool _gaveStar;
    [SerializeField] private DialogueAsset introDialogue;
    [SerializeField] private DialogueAsset fairyDialogue;

    private void Start()
    {
        DialogueSystem.onDialogueNextLine += CustomEvent;
    }

    public void Interact()
    {
        DialogueManager.instance.StartDialogue(talkedToOnce ? fairyDialogue : introDialogue);
        if (talkedToOnce) FairyStuff();
        talkedToOnce = true;

    }

    private void FairyStuff()
    {
        if (_gaveStar)
        {
            RuriMovement.instance.RemoveStar();
            _gaveStar = false;
        }
        else
        {
            RuriMovement.instance.AddStar();
            _gaveStar = true;
        }
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