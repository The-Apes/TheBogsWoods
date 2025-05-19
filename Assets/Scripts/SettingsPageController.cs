using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsPageController : MonoBehaviour
{
    public GameObject menuCanvas;

    public void OnResumeClick()
    {
        if (menuCanvas != null)
            menuCanvas.SetActive(false);
        PauseController.SetPause(false);
    }

    public void OnMainMenuClick()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void OnExitGameClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}