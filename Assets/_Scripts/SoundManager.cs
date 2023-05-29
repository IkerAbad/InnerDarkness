using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ConfigManager.instance != null)
        {
            audioSource.volume = ConfigManager.instance.soundV;
        }
        else
        {
            audioSource.volume = 0.1f;
        }
    }
}
