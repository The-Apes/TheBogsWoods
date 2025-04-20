using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceEffect : MonoBehaviour
{
    public float bounceHeight = 0.3f; // Height of the bounce
    public float bounceDuration = 0.4f; // Duration of the bounce
    public int bounceCount = 2; // Number of bounces

    public void StartBounce()
    {
        StartCoroutine(BounceHandler());
    }

    private IEnumerator BounceHandler()
    {
        Vector3 startPosition = transform.position;
        float localHeight = bounceHeight;
        float localDuration = bounceDuration;

        for (int i = 0; i < bounceCount; i++)
        {
            yield return Bounce(startPosition, localHeight, localDuration / 2);
            localHeight *= 0.5f; // Reduce height for each bounce
            localDuration *= 0.8f;
        }
        transform.position = startPosition; // Reset positionransform.position = startPosition; // Reset position
    }

    private IEnumerator Bounce( Vector3 start, float height, float duration)
    {
        Vector3 peak = start + Vector3.up * height;
        float elapsed = 0f; 
        
        //Move Up
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, peak, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        elapsed = 0f;

        //Move Down
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(peak, start, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
    }
}
