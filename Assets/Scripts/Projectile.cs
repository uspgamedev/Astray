using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public Transform tip;
    public GameObject shatterEffect;

    public float projectileSpeed = 20f;
    public int projectileDamage = 10;

    void Start()
    {
        rigidBody.velocity = transform.right * projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<AreaCamera>() == null &&
            collider.GetComponent<Boss>() == null &&
            collider.GetComponent<Bullet>() == null ) // Não é: câmera, Boss, Bala
        {
            Player player = collider.GetComponent<Player>();
            if (player != null) // Acertou o Player
            {
                player.TakeDamage(projectileDamage);
            }

            Instantiate(shatterEffect, tip.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}
