using UnityEngine;
using UnityEngine.Playables;

public class LevelZeroSceneManager : MonoBehaviour
{
    public DialogueTrigger startingDialogue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RuriMovement.instance.controlling = false;
        CutsceneManager.Instance.director.stopped += OnCutsceneStopped;
    }

    private void OnCutsceneStopped(PlayableDirector director)
    {
        if(director.playableAsset.name == "OttoRun")
        {
            DialogueManager.Instance.StartDialogue(startingDialogue.dialogue);
        }
    }
 
}
