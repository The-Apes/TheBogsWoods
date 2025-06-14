using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace CameraUtils
{
   public class MainCameraUtils : MonoBehaviour
   {
      private int _defaultZoom;
      private PixelPerfectCamera _pixelPerfectCamera;
      [SerializeField] private GameObject mainCamera;
      private void Awake()
      {
         _pixelPerfectCamera = mainCamera.GetComponent<PixelPerfectCamera>();
         _defaultZoom = _pixelPerfectCamera.assetsPPU;
      }
      public void SetZoom(int zoom, float duration = -1f)
      {
         if (!duration.Equals(-1f)) StartCoroutine(SetZoomCoroutine(zoom, duration));
      }

      private IEnumerator SetZoomCoroutine(int zoom, float duration = -1f)
      {
         int prevZoom = _pixelPerfectCamera.assetsPPU;
         _pixelPerfectCamera.assetsPPU = zoom;
         var actualDuration = duration.Equals(-1f) ? 0f : duration;
         yield return new WaitForSeconds(actualDuration);
         _pixelPerfectCamera.assetsPPU = prevZoom;
      }
   
      public void LerpZoom(int targetZoom,float speed = 2.5f, float duration = -1f)
      {
         StopAllCoroutines();
         StartCoroutine(LerpZoomCoroutine(targetZoom, speed, duration));
      }
      private IEnumerator LerpZoomCoroutine(int targetZoom, float speed, float duration)
      {
         int startZoom = _pixelPerfectCamera.assetsPPU;
         
         float elapsedTime = 0f;

         if (duration.Equals(-1f))
         {
            while (!startZoom.Equals(targetZoom))
            {
               elapsedTime += Time.deltaTime;
               float t = Mathf.Clamp01(elapsedTime * speed);
               _pixelPerfectCamera.assetsPPU = Mathf.RoundToInt(Mathf.Lerp(startZoom, targetZoom, t));
               yield return null;
            }
         }
         else
         {
            while (elapsedTime < duration)
            {
               elapsedTime += Time.deltaTime;
               float t = Mathf.Clamp01(elapsedTime * speed);
               _pixelPerfectCamera.assetsPPU = Mathf.RoundToInt(Mathf.Lerp(startZoom, targetZoom, t));
               yield return null;
            }
         }
         
         


         if (duration.Equals(-1f)) yield break; //basically return
         StartCoroutine(LerpZoomCoroutine(startZoom, speed, -1));
         print("I get here");
      }

      public void ResetZoom()
      {
         _pixelPerfectCamera.assetsPPU = _defaultZoom;
      }
   }
}
