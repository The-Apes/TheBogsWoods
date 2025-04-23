using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance;
    public PlayableDirector director;
    public GameObject normalCamera;
    public GameObject customCamera;
    
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
        customCamera = camera;
        normalCamera.SetActive(false);
        customCamera.SetActive(true);
        if(RuriMovement.instance != null) RuriMovement.instance.controlling = false;
    }

    private  void CutsceneStopped(PlayableDirector director)
    {
        if (!normalCamera.activeSelf){
            normalCamera.SetActive(true);
            customCamera.SetActive(false);
            if(RuriMovement.instance != null) RuriMovement.instance.controlling = true;

        }
    }

    
}
