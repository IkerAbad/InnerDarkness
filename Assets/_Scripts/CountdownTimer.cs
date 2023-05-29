using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    public float timeLimit = 300f;
    [SerializeField] TextMeshProUGUI countdownText;

    void FixedUpdate()
    {
        if (timeLimit != 0)
        {
            timeLimit -= Time.deltaTime;
            
        }
        else
        {
            // Game Over
            timeLimit = 0;
            SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings-1);
        }

        DisplayTime(timeLimit); 
    }

    void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        // Date Formatter
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float centiseconds = timeToDisplay % 1 * 100;

        // Display
        countdownText.text = "Time<br>" + string.Format("{00:00}:{01:00}:{02:00}", minutes, seconds, centiseconds);
    }
}
