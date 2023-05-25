using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float timeLimit = 300f;
    [SerializeField] TextMeshProUGUI countdownText;

    // Start is called before the first frame update
    void Start()
    {
    }

    void FixedUpdate()
    {
        if (timeLimit != 0)
        {
            timeLimit -= Time.deltaTime;
            
        }
        else
        {
            timeLimit = 0;
            // GAME OVER UI (BUTTONS)
                // RESTART LEVEL
                // QUIT
        }

        DisplayTime(timeLimit); 
    }

    void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float centiseconds = timeToDisplay % 1 * 100;

        countdownText.text = "Time<br>" + string.Format("{00:00}:{01:00}:{02:00}", minutes, seconds, centiseconds);
    }
}
