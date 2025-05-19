using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MainCamera : MonoBehaviour
{
   public static MainCamera instance;
   private int _defaultZoom;
   private PixelPerfectCamera _pixelPerfectCamera;
   private void Awake()
   {
      if (!instance)
      {
         instance = this;
      }
      else
      {
         Destroy(gameObject);
      }
      _pixelPerfectCamera = GetComponent<PixelPerfectCamera>();

      _defaultZoom = _pixelPerfectCamera.assetsPPU;
   }
   public void SetZoom(int zoom)
   {
      _pixelPerfectCamera.assetsPPU = zoom;
   }

   public void ResetZoom()
   {
      _pixelPerfectCamera.assetsPPU = _defaultZoom;
   }
}
