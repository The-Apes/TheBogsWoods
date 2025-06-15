using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class TestSpawnPotions : MonoBehaviour
{
    [SerializeField] private GameObject potion;
    public Vector2 groundDispenseVelocity;
    public Vector2 verticalDispenseVelocity;

    void Start()
    {
        StartCoroutine(SpawnPotion());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator SpawnPotion()
    {
        print(gameObject.transform.right);
        while (true)
        {
            GameObject instantiatedPotion = Instantiate(potion, gameObject.transform.position, Quaternion.identity);
            instantiatedPotion.GetComponent<FakeHeightObject>().Initialize(new Vector3(-1,0,0) * Random.Range(groundDispenseVelocity.x, groundDispenseVelocity.y), Random.Range(verticalDispenseVelocity.x, verticalDispenseVelocity.y));
            
            yield return new WaitForSeconds(1f);

        }


    }
}
