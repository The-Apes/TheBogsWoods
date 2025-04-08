using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas; // Reference to the menu canvas
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuCanvas.SetActive(!menuCanvas.activeSelf);
        }
    }
}
