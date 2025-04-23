using System;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance;
    private PlayableDirector _director;
    
    public static Action<string> CutsceneFinished;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        _director = GetComponent<PlayableDirector>();
    }
    public void PlayCutscene(PlayableAsset cutscene)
    {
        if (_director == null) _director = GetComponent<PlayableDirector>();
        _director.Play(cutscene);
    }
    _director
    public static void CutsceneFinished(string CustceneName) => CutsceneFinished?.Invoke(AreaName);

    
}
