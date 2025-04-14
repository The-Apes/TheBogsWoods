using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static bool IsGamePaused { get; private set; } = false;

    private void Start()
    {
        // Ensure the game is not paused when it starts
        IsGamePaused = false;
        Time.timeScale = 1;
    }

    public static void SetPause(bool pause)
    {
        IsGamePaused = pause;
        Time.timeScale = pause ? 0 : 1; // Pauses or unpauses the game
    }
}

