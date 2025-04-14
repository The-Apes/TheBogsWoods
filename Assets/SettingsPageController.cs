using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsPageController : MonoBehaviour
{
      public void OnMainMenuClick()
      {
         SceneManager.LoadScene("StartMenu");
      }
   public void onExitGameClick()
   {
      #if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
      #endif
      Application.Quit();
   }
}
