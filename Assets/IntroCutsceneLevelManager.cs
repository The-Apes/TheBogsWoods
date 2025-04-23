using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCutsceneLevelManager : MonoBehaviour
{
    public DialogueTrigger introCutsceneDialogue;
    public AudioClip music;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameEvents.OnDialogueEnd += OnDialogueEnd; // Subscribe to the dialogue end event

        DialogueManager.instance.StartDialogue(introCutsceneDialogue.dialogue);
        AudioManager.instance.PlaySound(music);
    }

    private void OnDialogueEnd(string dialogue)
    {
        SceneManager.LoadScene("Level 0");
    }
}
