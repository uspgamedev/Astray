using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour{
    
    public float speed;
    public float stoppingDistance;
    public float retreatDistance;
    public float activationDistance;
    public float tolerance;
    public Animator animator;
    
    private float timeBtwShots;
    public float startTimeBtwShots;
    
    public GameObject projectile;
    
    public Transform player;
    Vector2 movement;
    public Vector3 correcao;
    
    
    
    public Rigidbody2D rigidBody;
    public SpriteRenderer sprite;
    public GameObject deathEffect;

    public int maxHealth = 100;
    public int currentHealth;

    public int damage = 10;
    public float pushback = 50000;

    private float timer;
    private GameManager manager;


    public float startMovementMinX;
    public float startMovementMaxX;
    public float startMovementMinY;
    public float startMovementMaxY;
    public bool startMovement = false;
    public Vector3 posExtra1;
    public Vector3 posExtra2;
    
    
    
    void Start()
    {
        manager = FindObjectOfType<GameManager>();
    	player = GameObject.FindGameObjectWithTag("Player").transform;
    	currentHealth = maxHealth;
    	timeBtwShots = startTimeBtwShots;
    	transform.localPosition = posExtra1;
    	
    }

    // Update is called once per frame
    void Update()    {
        if (manager != null)
        {
            if (manager.playerAlive == false)
            {
                GetComponent<Enemy3>().enabled = false;
            }
        }
    }

    // Called after Update
    void LateUpdate()
    {	if (player.position.x > startMovementMinX && player.position.x < startMovementMaxX && player.position.y > startMovementMinY && player.position.y < startMovementMaxY && !startMovement){
    		startMovement = true;
    		transform.localPosition = posExtra2;
    		
    	}
    	if (startMovement){
        if (Vector2.Distance(transform.position,player.position)> stoppingDistance && Vector2.Distance(transform.position,player.position) < activationDistance){
    	
    		transform.position = Vector2.MoveTowards(transform.position, player.position,speed * Time.deltaTime);
    		movement.x = Input.GetAxisRaw("Horizontal");
        	movement.y = Input.GetAxisRaw("Vertical");        
        	animator.SetFloat("Horizontal", movement.x);
	    	animator.SetFloat("Vertical", movement.y);
	    	animator.SetFloat("Speed", movement.sqrMagnitude);
    	
    	} else if(Vector2.Distance(transform.position,player.position) < stoppingDistance && Vector2.Distance(transform.position,player.position) > retreatDistance){
    	
    		transform.position = this.transform.position;
    		
      		if (Vector2.Distance(transform.position,player.position) < stoppingDistance - tolerance && Vector2.Distance(transform.position,player.position) > retreatDistance + tolerance){
        		animator.SetFloat("Horizontal", 0);
	    		animator.SetFloat("Vertical", 0);
	    		animator.SetFloat("Speed", 0);
    		} else {
    			movement.x = Input.GetAxisRaw("Horizontal");
        		movement.y = Input.GetAxisRaw("Vertical");        
        		animator.SetFloat("Horizontal", movement.x);
	    		animator.SetFloat("Vertical", movement.y);
	    		animator.SetFloat("Speed", movement.sqrMagnitude);
    		}
    	
    	} else if(Vector2.Distance(transform.position,player.position) < retreatDistance){
    	
    		transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
    		
    		movement.x = Input.GetAxisRaw("Horizontal");
        	movement.y = Input.GetAxisRaw("Vertical");        
        	animator.SetFloat("Horizontal", movement.x);
	    	animator.SetFloat("Vertical", movement.y);
	    	animator.SetFloat("Speed", movement.sqrMagnitude);    		
    		
    	} else{
         	animator.SetFloat("Horizontal", 0);
	    	animator.SetFloat("Vertical", 0);
	    	animator.SetFloat("Speed", 0);
	
    	}
    	
    	if(timeBtwShots <= 0){
    	
    		Instantiate(projectile,transform.position + correcao ,Quaternion.identity);	
    		timeBtwShots = startTimeBtwShots;
    		
    	} else {
    	
    		timeBtwShots -=Time.deltaTime;
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
            player.TakeDamage(damage);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponent<Player>();
        if (player != null && Time.time - timer >= 0.5) // Dano a cada 0.5s
        {
            Debug.Log("Dano por contato");
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
    
    private void OnDrawGizmosSelected()
    {

        
        // Ativacao do personagem

        float drawStartMovementMinX = startMovementMinX;
        float drawStartMovementMaxX = startMovementMaxX;
        float drawStartMovementMinY = startMovementMinY;
        float drawStartMovementMaxY = startMovementMaxY;

        Vector3 topStartMovementLeft = new Vector3(drawStartMovementMinX, drawStartMovementMaxY);
        Vector3 topStartMovementRight = new Vector3(drawStartMovementMaxX, drawStartMovementMaxY);
        Vector3 bottomStartMovementLeft = new Vector3(drawStartMovementMinX, drawStartMovementMinY);
        Vector3 bottomStartMovementRight = new Vector3(drawStartMovementMaxX, drawStartMovementMinY);

        if (topStartMovementLeft.y < bottomStartMovementLeft.y || topStartMovementLeft.x > topStartMovementRight.x)
        {
            // Os valores mínimos e máximos não condizem
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.blue;
        }

        Gizmos.DrawLine(topStartMovementLeft, topStartMovementRight);
        Gizmos.DrawLine(topStartMovementLeft, bottomStartMovementLeft);
        Gizmos.DrawLine(topStartMovementRight, bottomStartMovementRight);
        Gizmos.DrawLine(bottomStartMovementLeft, bottomStartMovementRight);
        
       
    }
    
    
    
}


