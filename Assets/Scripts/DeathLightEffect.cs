using System.Collections;
using CameraUtils;
using Managers;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DeathLightEffect : MonoBehaviour
{
   private Animator animator;
   private Light2D light2D;

   private void Awake()
   {
      animator = GetComponent<Animator>();
      light2D = GetComponent<Light2D>();
   }

   private void Start()
   {
      CameraManager.instance.SetZoom(30);
      StartCoroutine(WaitForAnimation(animator.GetCurrentAnimatorStateInfo(0).length));;
   }
   
   IEnumerator WaitForAnimation(float time)
   {
      yield return new WaitForSeconds(time);
      GameController.instance.GameOverScreen();
   }
}
