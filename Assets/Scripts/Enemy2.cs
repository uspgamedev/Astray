using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public SpriteRenderer sprite;
    public GameObject deathEffect;

    public int maxHealth = 100;
    public int currentHealth;

    public int damage = 10;
    public float pushback = 50000;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        Player player = collider.GetComponent<Player>();
        if (player != null) // Encostou em um Player
        {
            timer = Time.time; // Momento do contato

            Rigidbody2D playerBody = collider.GetComponent<Rigidbody2D>();
            if (playerBody == null)
            {
                Debug.LogError("O Player não possui RigidBody2D");
            }
            else
            {
                Vector2 direction = collision.rigidbody.position - collision.otherRigidbody.position;
                playerBody.AddForce(direction.normalized * pushback); // Empurrão
            }

            Debug.Log(collision.otherCollider.name + " hit " + collider.name);
            player.TakeDamage(damage);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponent<Player>();
        if (player != null && Time.time - timer >= 0.5) // Dano a cada 0.5s
        {
            player.TakeDamage(damage);
            timer = Time.time; // Momento do dano por contato
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }

        StartCoroutine(BlinkEnemies());
    }

    IEnumerator BlinkEnemies()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }

    void Die()
    {
        Destroy(gameObject);
        Instantiate(deathEffect, transform.position, transform.rotation);
    }
}
