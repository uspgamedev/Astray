using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile2 : MonoBehaviour
{


    public Rigidbody2D rigidBody;
    public int bulletDamage = 10;
    public GameObject impactEffect;
    public float speed;
    public float pushback;
    private Transform player;
    private Vector2 target;

    
    // Start is called before the first frame update
    void Start()
    {
    	player = GameObject.FindGameObjectWithTag("Player").transform;
    	target = new Vector2(player.position.x,player.position.y);
    	
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position,target,speed*Time.deltaTime);
        
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<AreaCamera>() == null &&
            collider.CompareTag("Water") == false) // Não é uma câmera nem água
        {
            Debug.Log("Bullet hit " + collider.name);

            Enemy1 enemy1 = collider.GetComponent<Enemy1>();
            Enemy2 enemy2 = collider.GetComponent<Enemy2>();
            Enemy3 enemy3 = collider.GetComponent<Enemy3>();
            Boss boss = collider.GetComponent<Boss>();
            Player player = collider.GetComponent<Player>();
            if (enemy1 != null) // É um inimigo
            {
                enemy1.TakeDamage(bulletDamage);
            }
            else if (enemy2 != null)
            {
                enemy2.TakeDamage(bulletDamage);
            }
            else if (boss != null)
            {
                boss.TakeDamage(bulletDamage);
            }
            else if (enemy3 != null)
            {
            	enemy3.TakeDamage(bulletDamage);
            } else if(player != null){
                player.TakeDamage(bulletDamage);
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
