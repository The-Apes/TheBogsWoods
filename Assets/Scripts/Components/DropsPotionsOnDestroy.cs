using HeightObjects;
using Saving;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Components
{
    public class DropsPotion : MonoBehaviour
    {
        [SerializeField] private int amount;
    
        [SerializeField] private GameObject potion;
        public Vector2 groundDispenseVelocity;
        public Vector2 verticalDispenseVelocity;
    

        private void OnDestroy()
        {
            var comp = GetComponentInChildren<SaveableObject>();

            if (comp)
            {
                if(comp.selfDestroy) return;
            }
        
            for (int i = 0; i < amount; i++)
            {
                GameObject instantiatedPotion = Instantiate(potion, gameObject.transform.position, Quaternion.identity);
                instantiatedPotion.GetComponent<FakeHeightObject>().Initialize(new Vector3(Random.Range(-1f,1),Random.Range(-1f,1),0) * Random.Range(groundDispenseVelocity.x, groundDispenseVelocity.y), Random.Range(verticalDispenseVelocity.x, verticalDispenseVelocity.y));
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis

    }
}
