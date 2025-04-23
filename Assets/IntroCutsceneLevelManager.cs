using UnityEngine;

public class IntroCutsceneLevelManager : MonoBehaviour
{
    public DialogueTrigger introCutsceneDialogue;
    public AudioClip music;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DialogueManager.instance.StartDialogue(introCutsceneDialogue.dialogue);
        AudioManager.instance.PlaySound(music);
    }
}
