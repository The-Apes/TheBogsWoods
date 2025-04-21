using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance;
   
    private void Awake()
    {
        Instance = this;
    }

    public void HitStop(float duration)
    {
        StartCoroutine(HitStopRoutine(duration));
    }

     private IEnumerator  HitStopRoutine(float duration) {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
     }
}
