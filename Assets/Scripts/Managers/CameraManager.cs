using System;
using CameraUtils;
using UnityEngine;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager instance;
        private MainCameraUtils cameraUtils;
        private CinemachineUtils cmUtils;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }else
            {
                Destroy(gameObject);
            }

            cameraUtils = GetComponent<MainCameraUtils>();
            cmUtils = GetComponent<CinemachineUtils>();
        }

        public void LookAt(Transform target, float duration = -1f)
        {
            cmUtils.LookAt(target, duration);
        }
        public void LookAtLocation(Vector3 target, float duration = -1f)
        {
            cmUtils.LookAtLocation(target, duration);
        }

        public void LerpZoom(int targetZoom,float speed = 2.5f, float duration = -1f)
        {
            cameraUtils.LerpZoom(targetZoom, speed, duration);
        }

        public void SetZoom(int zoom, float duration = -1f)
        {
            cameraUtils.SetZoom(zoom, duration);
        }
    }
}
