using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Over")]
    public GameObject gameOverPanel;
    public Text winText;
    public GameObject rematchButton;
    public GameObject mainMenuButton;
    private bool gameIsOver = false;

    [Header("Count Down")]
    public Text countdownText;
    public float countdownDuration = 3f;
    public float fightDisplayDuration = 1f; // Duration to display "Fight!" text

    private float currentCountdownValue;
    private bool countdownFinished = false;

    public AudioClip victorySound;
    public AudioClip countdownSound;
    public AudioClip backgroundMusic;

    private void Start()
    {
        gameOverPanel.SetActive(false);
        StartCoroutine(StartCountdown());
        AudioManager.instance.PlayBackgroundMusic(backgroundMusic);

    }

    public void PlayerWins(int playerNumber)
    {
        if (!gameIsOver)
        {
            gameIsOver = true;
            Time.timeScale = 0f; // Pause the game

            AudioManager.instance.PlayBackgroundMusic(victorySound);
            // Display win message for the winning player
            winText.text = "Player " + playerNumber + " Wins!";

            // Show restart and main menu buttons
            gameOverPanel.SetActive(true);
        }
    }

    public void RestartGame()
    {
        // Reload the current scene to restart the game
        gameIsOver = false;
        Time.timeScale = 1f; // Unpause the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        // Load the main menu scene
        Time.timeScale = 1f; // Unpause the game
        SceneManager.LoadScene("Main Menu"); 
    }


    IEnumerator StartCountdown()
    {
        Time.timeScale = 0f;
        // Display "Ready?" for 2 seconds before starting the countdown
        countdownText.text = "Ready?";
        yield return new WaitForSecondsRealtime(2f);

        // Reset countdown value
        currentCountdownValue = countdownDuration;

        //start countdown audio
        AudioManager.instance.PlaySoundEffect(countdownSound);

        // Start the countdown
        while (currentCountdownValue > 0)
        {
            
            countdownText.text = currentCountdownValue.ToString();
            yield return new WaitForSecondsRealtime(1f);
            currentCountdownValue--;
        }

        // Countdown finished, display "Fight!" text
        countdownText.text = "Fight!";
        yield return new WaitForSecondsRealtime(fightDisplayDuration);

        // After displaying "Fight!" for a short duration, resume the game
        Time.timeScale = 1f;

        // Hide the countdown text
        countdownText.gameObject.SetActive(false);
        countdownFinished = true;
    }

    public bool IsCountdownFinished()
    {
        return countdownFinished;
    }
}
