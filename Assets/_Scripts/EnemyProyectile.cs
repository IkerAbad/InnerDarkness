using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProyectile : MonoBehaviour
{
    public int damage = 5;  // Proyectile damage

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))  // If proyectile hits player
        {
            //Debug.Log("Player collision!");
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.TakeDamage(damage);
        }

        Destroy(gameObject);  // Destroys projectile if it collides
    }
}
