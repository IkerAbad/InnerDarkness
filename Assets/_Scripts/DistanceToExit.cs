using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DistanceToExit : MonoBehaviour
{
    public GameObject player;
    public GameObject exit;
    public float distance;
    [SerializeField] TextMeshProUGUI distanceText;

    void Update()
    {
        distance = Vector3.Distance(player.transform.position, exit.transform.position);
        if (distance < 2)
        {
            //PlayerPrefs.SetInt("Nivel",2);
            // load the nextlevel
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        distanceText.text = "Distance<br>" + distance.ToString("00") + " m";
    }
}
