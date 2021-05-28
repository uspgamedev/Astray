using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public Boss bossLife;
    public Rigidbody2D rigidBody;
    public Animator animator;
    public GameObject projectilePreFab;
    public Transform attackPoint;
    public AudioSource audioSource;

    public float attackRange = 2f;
    public int punchDamage = 20;
    public float cooldown = 1f;

    private float lastAttack;
    private ParticleSystem effect;

    void Start()
    {
        effect = attackPoint.GetComponent<ParticleSystem>();
        if (effect == null)
        {
            Debug.LogError("The Boss' attackPoint doesn't have a particle system");
        }
        lastAttack = Time.time;
    }

    void LateUpdate()
    {
        if (bossLife.isHalfLife == false)
        {
            AttackStage1();
        }
        else if (animator.GetBool("Glow") == false)
        {
            AttackStage2();
        }
    }

    void AttackStage1()
    {
        GameObject existingProjectile = GameObject.FindGameObjectWithTag("Projectile");
        if (existingProjectile != null) // O braço está fora 
        {
            ResetShoot();
            rigidBody.velocity = Vector2.zero; // Boss parado
        }
        else // Atirar
        {
            animator.SetBool("ShootArm", true);
        }
    }

    void AttackStage2()
    {
        if (Time.time > lastAttack + cooldown) // Atacar
        {
            bossLife.isImovable = true;
            animator.SetBool("PunchGround", true);
        }
    }

    void Shoot()
    {
        Instantiate(projectilePreFab, transform.position, transform.rotation);
    }

    void ResetShoot()
    {
        animator.SetBool("ShootArm", false);
    }

    void PunchGround()
    {
        effect.Play();
        audioSource.Play();
        lastAttack = Time.time;

        Collider2D[] hitList = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        foreach (Collider2D objectHit in hitList)
        {
            Player player = objectHit.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(punchDamage);
            }

            Bullet bullet = objectHit.GetComponent<Bullet>();
            if (bullet != null)
            {
                Destroy(objectHit.gameObject);
            }
        }
    }

    void ResetPunch()
    {
        animator.SetBool("PunchGround", false);
        bossLife.isImovable = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            Debug.LogError("Boss' attackPoint not assigned");
        }
        else
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
