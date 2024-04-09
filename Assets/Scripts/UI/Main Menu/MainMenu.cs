using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public AudioClip titleTheme;
    public AudioClip uiSound;
    private void Start()
    {
        AudioManager.instance.PlayBackgroundMusic(titleTheme);
    }
    // Loads Character Select when BattleButton is pressed
    public void PlayGame()
    {
        SceneManager.LoadScene("LevelOne");
    }

    //**NOTE: Tutorial is handled without code via the editor.

    //Closes the game when quit is pressed
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Pressed.");
    }

    public void PlayUISound()
    {
        AudioManager.instance.PlaySoundEffect(uiSound);
    }
}
