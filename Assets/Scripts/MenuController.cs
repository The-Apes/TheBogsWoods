using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;

    private void Start()
    {
        menuCanvas.SetActive(false);
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        if (!menuCanvas.activeSelf && PauseController.IsGamePaused)
        {
            return;
        }
        ToggleMenu();
    }

    public void ToggleMenu()
    {
        bool showMenu = !menuCanvas.activeSelf;
        menuCanvas.SetActive(showMenu);
        PauseController.SetPause(showMenu);
    }

    public void ResumeGame()
    {
        menuCanvas.SetActive(false);
        PauseController.SetPause(false);
    }
}