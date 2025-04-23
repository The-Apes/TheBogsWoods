using UnityEngine;

public class LevelZeroSceneManager : MonoBehaviour
{
    public DialogueTrigger startingDialogue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DialogueManager.instance.StartDialogue(startingDialogue.dialogue);
    }

 
}
