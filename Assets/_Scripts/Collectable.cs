using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    // Player walks into collectable
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player)
        {
            // Add health to player
            player.health += 5;

            // Delete collectable from the screen
            Destroy(this.gameObject);
        }
    }
}