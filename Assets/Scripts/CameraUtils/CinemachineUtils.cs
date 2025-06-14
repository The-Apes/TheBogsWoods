using System.Collections;
using Unity.Cinemachine;
using UnityEngine;


namespace CameraUtils
{
    
    public class CinemachineUtils : MonoBehaviour
    {
        [SerializeField] private GameObject mainCamera;
        CinemachineCamera cam;
        private GameObject customFollowTarget;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
          cam = mainCamera.GetComponent<CinemachineCamera>();
          customFollowTarget = new GameObject
          {
              name = "CinemachineFollowTarget"
          };
        }

        public void LookAt(Transform target, float duration = -1f)
        {
            StartCoroutine(LookAtCoroutine(target, duration));
        }
        public void LookAtLocation(Vector3 target, float duration = -1f)
        {
            StartCoroutine(LookAtLocationCoroutine(target, duration));
        }
        
        private IEnumerator LookAtCoroutine(Transform target, float duration)
        {
            Transform prevTarget = cam.Follow;
            cam.Follow = target;
            if (duration.Equals(-1f)) yield break;
            yield return new WaitForSeconds(duration);
            cam.Follow = prevTarget;
            
        }
        private IEnumerator LookAtLocationCoroutine(Vector3 target, float duration)
        {
            Transform prevTarget = cam.Follow;
            customFollowTarget.transform.position = target;
            cam.Follow = customFollowTarget.transform;
            if (duration.Equals(-1f)) yield break;
            yield return new WaitForSeconds(duration);
            cam.Follow = prevTarget;
            
        }
       
    }
}
