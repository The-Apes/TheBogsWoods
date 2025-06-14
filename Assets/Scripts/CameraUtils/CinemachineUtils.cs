using System.Collections;
using Unity.Cinemachine;
using UnityEngine;


namespace CameraUtils
{
    
    public class CinemachineUtils : MonoBehaviour
    {
        [SerializeField] private GameObject mainCamera;
        private CinemachineCamera _cam;
        private CinemachinePositionComposer _composer;
        private GameObject _customFollowTarget;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
          _cam = mainCamera.GetComponent<CinemachineCamera>();
          _composer = mainCamera.GetComponent<CinemachinePositionComposer>();
          _customFollowTarget = new GameObject
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
            Transform prevTarget = _cam.Follow;
            _cam.Follow = target;
            if (duration.Equals(-1f)) yield break;
            yield return new WaitForSeconds(duration);
            _cam.Follow = prevTarget;
            
        }
        private IEnumerator LookAtLocationCoroutine(Vector3 target, float duration)
        {
            Transform prevTarget = _cam.Follow;
            _customFollowTarget.transform.position = target;
            _cam.Follow = _customFollowTarget.transform;
            if (duration.Equals(-1f)) yield break;
            yield return new WaitForSeconds(duration);
            _cam.Follow = prevTarget;
            
        }
        
        public void SetDamping(float damping)
        {
            _composer.Damping = new Vector3(damping, damping, damping);
        }
       
    }
}
