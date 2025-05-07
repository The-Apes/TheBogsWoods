using UnityEngine;

public class Transition : MonoBehaviour
{
    // The location to move the camera to when the player enters the trigger
    public Vector3 sceneLocation= new Vector3(0, 1, -10); 
    public Camera mainCamera; 
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        mainCamera.transform.position = sceneLocation;
        if (other.CompareTag("Player"))
        {
            // find or get main camera
        }
    }
}
