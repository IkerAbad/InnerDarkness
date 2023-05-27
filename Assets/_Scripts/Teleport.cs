using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Vector3 position;
    [SerializeField] private int escena;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //PlayerPrefs.SetInt("Nivel",escena);
            //SceneManager.LoadScene(escena);
            
            col.gameObject.transform.position = position;
        }
    }
}
