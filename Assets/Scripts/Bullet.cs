using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public int bulletDamage = 10;
    public GameObject impactEffect;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<AreaCamera>() == null &&
            collider.GetComponent<Player>() == null ) // Não é uma câmera nem o Player
        {
            Enemy1 enemy1 = collider.GetComponent<Enemy1>();
            Enemy2 enemy2 = collider.GetComponent<Enemy2>();
            Enemy3 enemy3 = collider.GetComponent<Enemy3>();
            Enemy4 enemy4 = collider.GetComponent<Enemy4>();
            Boss boss = collider.GetComponent<Boss>();
            if (enemy1 != null) // É um inimigo
            {
                enemy1.TakeDamage(bulletDamage);
            }
            else if (enemy2 != null)
            {
                enemy2.TakeDamage(bulletDamage);
            }
            else if (enemy3 != null)
            {
                enemy3.TakeDamage(bulletDamage);
            }
            else if (enemy4 != null)
            {
                enemy4.TakeDamage(bulletDamage);
            }
            else if (boss != null)
            {
                boss.TakeDamage(bulletDamage);
            }

            Destroy(gameObject);

            Instantiate(impactEffect, transform.position, transform.rotation);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
