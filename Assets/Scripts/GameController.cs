using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject gameOverScreen; // Reference to the Game Over screen UI
    public TMP_Text youAreDeadText; // Reference to the "You Are Dead" text UI
    
    [SerializeField] private GameObject deathLightEffectPrefab;

    public static event Action OnReset; // Event to notify when the game is reset
    
    public static GameController instance; // Singleton instance of GameController

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        PlayerHealth.OnPlayerDied += PlayerDied;
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
        if (instance == null)
        {
            instance = this; // Assign the singleton instance
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    private void OnDestroy()
    {
        PlayerHealth.OnPlayerDied -= PlayerDied;
    }

    /// <summary>
    /// Displays the game over screen, stops time, and shows the "You are dead!" message.
    /// </summary>
    private void PlayerDied()
    {
        RuriMovement.instance.Death();
        Vector3 deathLightPosition = RuriMovement.instance.transform.position;
        deathLightPosition.y += 0.25f;
        Instantiate(deathLightEffectPrefab, deathLightPosition, Quaternion.identity);
    }
    public void GameOverScreen()
    {
        Time.timeScale = 0;
        if (gameOverScreen)
        {
            gameOverScreen.SetActive(true);
        }
        
        if (youAreDeadText)
        {
            youAreDeadText.text = "You are dead!";
        }
    }
    
   public void OnRetryClick()
    {
        ResetGame();
        // Reload the current scene
        SceneChangeManager.instance.Restart();
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

