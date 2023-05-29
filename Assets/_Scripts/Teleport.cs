using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Vector3 position;
    [SerializeField] private int scene;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        // If player enters teleportation zone
        if (col.gameObject.CompareTag("Player"))
        {
            // Teleport
            col.gameObject.transform.position = position;
        }
    }
}
