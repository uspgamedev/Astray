using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public SpriteRenderer sprite;
    public BossAttack attack;
    public Animator animator;
    public CircleCollider2D circleCollider;
    public BoxCollider2D areaOfActivation;

    public int maxHealth = 700;
    public int currentHealth;
    public bool isSleeping;
    public bool isImovable;
    public bool isVulnerable;
    public bool isHalfLife;

    public int touchDamage = 10;
    public float pushback = 50000;

    private float timer;
    private GameManager manager;
    private LayerMask player;
    private ContactFilter2D playerFilter;
    private Collider2D[] objectsOnArea = new Collider2D[1];

    // Start is called before the first frame update
    void Start()
    {
        player = LayerMask.GetMask("Player");
        playerFilter.SetLayerMask(player);
        playerFilter.useLayerMask = true;
        playerFilter.useTriggers = true;

        manager = FindObjectOfType<GameManager>();
        isSleeping = true;
        isImovable = false;
        isVulnerable = false;
        isHalfLife = false;
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (areaOfActivation != null)
        {
            if (isSleeping == true &&
                areaOfActivation.OverlapCollider(playerFilter, objectsOnArea) == 1)
            {
                FindObjectOfType<AudioManager>().Play("BossTheme");
                animator.SetTrigger("WakeUp");
                isSleeping = false;
            }
        }
        else
        {
            Debug.LogError("The Boss doesn't have an area of activation");
        }

        if (manager != null)
        {
            if (manager.playerAlive == false)
            {
                GetComponent<BossMovement>().enabled = false;
                GetComponent<BossAttack>().enabled = false;
            }
        }
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
            player.TakeDamage(touchDamage);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponent<Player>();
        if (player != null && Time.time - timer >= 0.5) // Dano a cada 0.5s
        {
            player.TakeDamage(touchDamage);
            timer = Time.time; // Momento do dano por contato
        }
    }

    public void TakeDamage(int damage)
    {
        if (isVulnerable == true) 
        {
            currentHealth -= damage;
            StartCoroutine(BlinkEnemies());

            if (isHalfLife == false && currentHealth <= maxHealth / 2 )
            {
                isHalfLife = true;
                AngryGlow();
            }

            if (currentHealth <= 0)
            {
                isImovable = true;
                isVulnerable = false;
                circleCollider.enabled = false;
                sprite.sortingOrder = 2;
                animator.SetTrigger("Die");
            }
        }
    }

    IEnumerator BlinkEnemies()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }

    public void AngryGlow()
    {
        isVulnerable = false;
        animator.SetBool("Glow", true);
    }

    void Die()
    {
        if (manager != null)
        {
            manager.WinGame();
        }
        Destroy(gameObject);
    }

    public void StartAttack()
    {
        isVulnerable = true;
        animator.SetTrigger("StartAttack");
    }

    public void SecondStage()
    {
        animator.SetBool("Glow", false);
        isVulnerable = true;
    }
}
