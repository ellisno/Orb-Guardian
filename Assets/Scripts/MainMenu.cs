using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    // Loads Character Select when BattleButton is pressed
    public void PlayGame()
    {
        SceneManager.LoadScene("CharacterSelect");
    }

    //**NOTE: Options is handled without code via the editor.

    //Closes the game when quit is pressed
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Pressed.");
    }
}
