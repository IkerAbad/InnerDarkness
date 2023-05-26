using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public UiManager ui;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);
    }
    /*
    public void GameOver()
    {
        restartButton.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
    }
     */
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
   
    public void MenuDestroy()
    {
        Debug.Log("Menu");
        Destroy(this.gameObject);
    }
    public UiManager GetUi()
    {
        return ui;
    }
}

