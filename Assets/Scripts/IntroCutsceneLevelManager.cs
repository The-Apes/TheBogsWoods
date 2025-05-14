using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCutsceneLevelManager : MonoBehaviour
{
    public DialogueAsset introCutsceneDialogue;
    public AudioClip music;
    
    private void Start()
    {
        GameEvents.onDialogueEnd += OnDialogueEnd; // Subscribe to the dialogue end event
        Debug.Log("IntroCutsceneLevelManager");
        DialogueManager.instance.StartDialogue(introCutsceneDialogue.dialogue);
        Debug.Log("Tried to start the scene");
        AudioManager.instance.PlayMusic(music);
    }

    private void OnDialogueEnd(string dialogue)
    {
        AudioManager.instance.StopMusic();
        SceneManager.LoadScene("Level 0");
    }
    private void OnDestroy()
    {
        GameEvents.onDialogueEnd -= OnDialogueEnd; // Unsubscribe from the dialogue end event
    }

    private void OnDisable()
    {
        GameEvents.onDialogueEnd -= OnDialogueEnd; // Unsubscribe from the dialogue end event
    }
}
