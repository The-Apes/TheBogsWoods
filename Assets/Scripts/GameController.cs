using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject gameOverScreen; // Reference to the Game Over screen UI
    public TMP_Text youAreDeadText; // Reference to the "You Are Dead" text UI

    public static event Action OnReset; // Event to notify when the game is reset

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        PlayerHealth.OnPlayerDied += GameOverScreen;
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        PlayerHealth.OnPlayerDied -= GameOverScreen;
    }

    /// <summary>
    /// Displays the game over screen, stops time, and shows the "You are dead!" message.
    /// </summary>
    private void GameOverScreen()
    {
        Time.timeScale = 0;
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        if (youAreDeadText != null)
        {
            youAreDeadText.text = "You are dead!";
        }
    }
   public void OnRetryClick()
    {
        ResetGame();
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        OnReset?.Invoke();
    }
    private void ResetGame()
    {
        Time.timeScale = 1; // Resume the game time
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
    }

 
}

