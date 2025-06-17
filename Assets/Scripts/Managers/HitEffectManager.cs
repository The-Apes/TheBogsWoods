using UnityEngine;
using System.Collections;

public class HitEffectManager : MonoBehaviour
{
    public static HitEffectManager Instance;

    public GameObject hitParticlePrefab;
    public float hitStopDuration = 0.05f;
    public float shakeDuration = 0.1f;
    public float shakeMagnitude = 0.15f;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayHitEffects(Vector2 hitPoint, Vector2 hitDirection)
    {
        GameObject hitFX = Instantiate(hitParticlePrefab, hitPoint, Quaternion.identity);
        hitFX.transform.right = hitDirection.normalized;

        StartCoroutine(CameraShake.Instance.Shake(shakeDuration, shakeMagnitude));
        StartCoroutine(HitStop());
    }

    private IEnumerator HitStop()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(hitStopDuration);
        Time.timeScale = 1f;
    }
}