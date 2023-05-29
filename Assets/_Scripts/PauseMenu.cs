using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused;

    void Start()
    {
        pauseMenu.SetActive(false);

    }

    void Update()
    {
        // ESC Key to pause and unpause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Pause game function
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        // Pause time in game
        Time.timeScale = 0f;
        isPaused = true;
    }

    // Resume game function
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        // Unpause time in game
        Time.timeScale = 1f;
        isPaused = false;
    }

    // Exit to the main menu function
    public void ExitToMainMenu()
    {
        isPaused = false;
        // Unpause time in game
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }


}
