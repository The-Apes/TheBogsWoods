using System;
using Player;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager instance;
    public PlayableDirector director;
    public GameObject normalCamera;
    public GameObject customCamera;
    
    public static Action<string> cutsceneFinished;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        director = GetComponent<PlayableDirector>();
        director.stopped += CutsceneStopped;

    }
    
    /// <summary>
    /// Plays a cutscene (without changing the camera).
    /// </summary>
    /// <param name="cutscene">The cutscene to be played.</param>
    public void PlayCutscene(PlayableAsset cutscene)
    {
        if (director == null) director = GetComponent<PlayableDirector>();
        director.Play(cutscene);
    }
    /// <summary>
    /// Plays a cutscene and switches to a custom camera.
    /// Disables the normal camera and disables player control while the cutscene is active.
    /// </summary>
    /// <param name="cutscene">The cutscene to be played.</param>
    /// <param name="newCamera">The custom camera to be used during the cutscene.</param>
    public void PlayCutscene(PlayableAsset cutscene, GameObject newCamera)
    {
        PlayCutscene(cutscene);
        customCamera = newCamera;
        normalCamera.SetActive(false);
        customCamera.SetActive(true);
        if(RuriMovement.instance != null) RuriMovement.instance.controlling = false;
    }

    /// <summary>
    /// Called when the cutscene stops. It restores the normal camera,
    /// disables the custom camera, and re-enables player control if needed.
    /// </summary>
    /// <param name="initiatingDirector">The director that triggered the cutscene to stop.</param>
    private  void CutsceneStopped(PlayableDirector initiatingDirector)
    {
        if (!normalCamera.activeSelf){
            normalCamera.SetActive(true);
            customCamera.SetActive(false);
        }
        if(RuriMovement.instance != null) RuriMovement.instance.controlling = true; 
        //maybe let this be decided per cutscene and not by default
    }

    
}
