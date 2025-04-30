using Managers;
using UnityEngine;
using UnityEngine.Playables;

public class LevelZeroSceneManager : MonoBehaviour
{
    public DialogueAsset startingDialogue;
    public DialogueAsset encounterDialogue;
    public PlayableAsset startingCutscene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RuriMovement.instance.controlling = false;
        CutsceneManager.Instance.director.stopped += OnCutsceneStopped;
        CutsceneManager.Instance.PlayCutscene(startingCutscene);
    }

    private void OnCutsceneStopped(PlayableDirector director)
    {
        if(director.playableAsset.name == "OttoRun")
        {
            DialogueManager.Instance.StartDialogue(startingDialogue.dialogue);
        }

        if (director.playableAsset.name == "Monkey Encounter")
        {
            DialogueManager.Instance.StartDialogue(encounterDialogue.dialogue);
        }
    }
 
}
