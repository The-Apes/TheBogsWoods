using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    public PlayableAsset cutscene;
    public GameObject cutsceneCamera;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        CutsceneManager.instance.PlayCutscene(cutscene,cutsceneCamera);
        Destroy(gameObject);
    }
}
