using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountdownTimer : MonoBehaviour
{
    public Text countdownText;
    public GameObject player; // Reference to the player GameObject
    public float countdownDuration = 3f;
    public float fightDisplayDuration = 1f; // Duration to display "Fight!" text

    private float currentCountdownValue;
    private bool countdownFinished = false;

    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        currentCountdownValue = countdownDuration;

        // Pause the game and prevent player movement
        Time.timeScale = 0f;
        player.GetComponent<Player>().enabled = false; // Disable player movement

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
        player.GetComponent<Player>().enabled = true; // Enable player movement

        // Hide the countdown text
        countdownText.gameObject.SetActive(false);
        countdownFinished = true;
    }

    public bool IsCountdownFinished()
    {
        return countdownFinished;
    }
}
