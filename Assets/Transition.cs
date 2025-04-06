using UnityEngine;

public class Transition : MonoBehaviour
{
    public Vector3 sceneLocation= new Vector3(0, 1, -10); // The location to move the camera to when the player enters the trigger
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Camera mainCamera; // The camera to move
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        mainCamera.transform.position = sceneLocation;
        if (other.CompareTag("Player"))
        {
            // find or get main camera
            
        }
    }
}
