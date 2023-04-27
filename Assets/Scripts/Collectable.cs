using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    // Player walks into collectable

    // Add collectable to player

    // Delete collectable from the screen

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        // If the player gets the gun, playerIsArmed is set to true
        if (player)
        {
            player.playerIsArmed = true;
            Destroy(this.gameObject);
        }
    }
}
