using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class IntroCutsceneLevelManager : MonoBehaviour
{
    public DialogueTrigger introCutsceneDialogue;
    public AudioClip music;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameEvents.OnDialogueEnd += OnDialogueEnd; // Subscribe to the dialogue end event
        Debug.Log("IntroCutsceneLevelManager");
        DialogueManager.Instance.StartDialogue(introCutsceneDialogue.dialogue);
        Debug.Log("Tried to start the scene");
        AudioManager.instance.PlaySound(music);
    }

    private void OnDialogueEnd(string dialogue)
    {
        SceneManager.LoadScene("Level 0");
    }
    private void OnDestroy()
    {
        GameEvents.OnDialogueEnd -= OnDialogueEnd; // Unsubscribe from the dialogue end event
    }

    private void OnDisable()
    {
        GameEvents.OnDialogueEnd -= OnDialogueEnd; // Unsubscribe from the dialogue end event
    }
}
