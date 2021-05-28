using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public SpriteRenderer sprite;
    public HealthBar healthBar;
    public int maxHealth = 100;
    public int currentHealth;
    public List<Key> keys;
    public int commonKeysAmount;
    public KeyUI keyUI;

    public AudioSource hitSound;
    public AudioSource healSound;
    public AudioSource keySound;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        commonKeysAmount = 0;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (damage < 0)
        {
            StartCoroutine(BlinkPlayerG());
            healSound.Play();
        }
        else
        {
            StartCoroutine(BlinkPlayerR());
            hitSound.Play();
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    IEnumerator BlinkPlayerR()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }
    
    
    IEnumerator BlinkPlayerG()

    {
        sprite.color = Color.green;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }    
    

    void Die()
    {
        GameManager manager = FindObjectOfType<GameManager>();
        if (manager != null)
        {
            manager.GameOver();
        }
        Destroy(gameObject);
    }
}
