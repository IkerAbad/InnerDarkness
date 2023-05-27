using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProyectile : MonoBehaviour
{
    public int damage = 5;  // Daño que hace el proyectil

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))  // Si el proyectil golpea al jugador
        {
            Debug.Log("Player collision!");
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.TakeDamage(damage);
        }

        Destroy(gameObject);  // Destruye el proyectil cuando choca con cualquier cosa
    }
}
