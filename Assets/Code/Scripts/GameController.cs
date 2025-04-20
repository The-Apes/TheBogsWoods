using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject gameOverScreen; // Reference to the Game Over screen UI
    public TMP_Text YouAreDeadText; // Reference to the "You Are Dead" text UI

    public static event Action OnReset; // Event to notify when the game is reset

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerHealth.OnPlayerDied += GameOverScreen;
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false); // Hide the Game Over screen at the start
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the event to avoid accessing destroyed objects
        PlayerHealth.OnPlayerDied -= GameOverScreen;
    }

    void GameOverScreen()
    {
        Time.timeScale = 0;
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        if (YouAreDeadText != null)
        {
            YouAreDeadText.text = "You are dead!";
        }
    }

    public void ResetGame()
    {
        Time.timeScale = 1; // Resume the game time
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
    }

    public void onRetryClick()
    {
        ResetGame();
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        OnReset.Invoke();
    }
}

