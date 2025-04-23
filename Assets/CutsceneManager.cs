using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance;
    public PlayableDirector director;
    
    public static Action<string> CutsceneFinished;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        director = GetComponent<PlayableDirector>();
        director.stopped += CutsceneStopped;

    }
    public void PlayCutscene(PlayableAsset cutscene)
    {
        if (director == null) director = GetComponent<PlayableDirector>();
        director.Play(cutscene);
    }
    public void PlayCutscene(PlayableAsset cutscene, GameObject camera)
    {
        PlayCutscene(cutscene);
    }

    public static void CutsceneStopped(PlayableDirector director)
    {
        print(director.playableAsset.name);
    }

    
}
