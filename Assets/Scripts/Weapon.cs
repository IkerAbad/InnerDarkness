using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 10f;
    public float projectileSpeed = 1f;
    public float damageBonus = 0f;

    [SerializeField] private Rigidbody2D _rb = null;
    [SerializeField] private GameObject _player = null;
    [SerializeField] private Transform _firePoint = null;
    [SerializeField] private GameObject _projectile = null;

    public void Shoot()
    {
        GameObject projectile = Instantiate(_projectile, _firePoint.position, _firePoint.rotation); // instantiate projectile and make it a child of weapon
        Rigidbody2D projectile_rb = projectile.transform.GetComponent<Rigidbody2D>();

        //projectile_rb.AddForce(_firePoint.up * projectileSpeed, ForceMode2D.Impulse);
        projectile_rb.velocity = _firePoint.up * projectileSpeed;
    }

    public void GetEquiped()
    {
        Transform playerTransform = _player.transform;
        float playerLocalOffset = _player.GetComponent<BoxCollider2D>().size.x;

        _rb.isKinematic = true; // makes this move with the player
        transform.rotation = playerTransform.rotation;
        transform.SetParent(playerTransform);
        transform.localPosition = new Vector3(-playerLocalOffset, 0, playerTransform.position.z);
    }

    public void GetUnequiped()
    {
        _rb.isKinematic = false; // makes it dynamic agian
        transform.SetParent(null);
        transform.position = new Vector3(transform.position.x, transform.position.y, 2);
    }
}

