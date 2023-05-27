using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Slider volum;
    public Slider sfx;
    public bool optionsOpen = false;
    void Start()
    {
        if(GameManager.instance != null)
            GameManager.instance.MenuDestroy();
        if (volum != null)
        {
            volum.value = ConfigManager.instance.soundV;
            sfx.value = ConfigManager.instance.sfxV;
        }

    }
    public void StartGame()
    {
        if (!PlayerPrefs.HasKey("Nivel"))
        {
            PlayerPrefs.SetInt("Nivel",1);
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("Nivel"));
        }
    }
    
    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void UpdateSound()
    {
        if(optionsOpen == true)
        {
            ConfigManager.instance.sfxV = sfx.value;
            ConfigManager.instance.soundV = volum.value;
            Debug.Log(ConfigManager.instance.sfxV);
            Debug.Log(ConfigManager.instance.soundV);
        }
    }
    public void menuOpen(bool options)
    {
        optionsOpen = options;
    }
}
