using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
  public void OnStartClick()
  {
    SceneManager.LoadScene("Level 0");
  }

  public void OnExitClick()
  {
    #if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
    #endif
    Application.Quit();
  }
}
