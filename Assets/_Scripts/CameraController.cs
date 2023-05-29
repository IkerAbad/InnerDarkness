using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0, 0, -1f);

    void LateUpdate()
    {
        // Follow Player
        transform.position = player.transform.position + offset;
    }
}
