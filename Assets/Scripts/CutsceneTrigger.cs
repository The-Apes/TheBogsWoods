using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class CutsceneTrigger : MonoBehaviour
{
    public PlayableAsset cutscene;
    public GameObject cutsceneCamera;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CutsceneManager.Instance.PlayCutscene(cutscene,cutsceneCamera);
            Destroy(gameObject);
        }
    }
}
