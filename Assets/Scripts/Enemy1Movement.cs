using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Movement : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Animator animator;
    public CircleCollider2D circleCollider;

    public float moveSpeed = 2f;
    public float startWaitTime;
    private float waitTime;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private Vector2 moveSpot;
    private float distance;
    
    
    // Ativacao do personagem
    public float startMovementMinX;
    public float startMovementMaxX;
    public float startMovementMinY;
    public float startMovementMaxY;
    public bool startMovement = false;
    private GameManager manager;
    private Transform target;    

    void Start()
    {
        waitTime = startWaitTime;
        moveSpot = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY)); // Ponto aleatório dentro das coordenadas
        
        // Ativacao do personagem
        manager = FindObjectOfType<GameManager>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();  

    }
    
    void Update()
    {
        if (manager != null)
        {
            if (manager.playerAlive == false)
            {
                GetComponent<Enemy1Movement>().enabled = false;
            }
        }
    }
    
    void LateUpdate()
    {	
    	if (target.position.x > startMovementMinX && target.position.x < startMovementMaxX && target.position.y > startMovementMinY && target.position.y < startMovementMaxY){
    		startMovement = true;
    	}
    
    
    	if (startMovement){
    	
        	distance = Vector2.Distance(transform.position, moveSpot);
        	transform.position = Vector2.MoveTowards(transform.position, moveSpot, moveSpeed * Time.deltaTime);

        	if (transform.position.x < moveSpot.x)
        	{
            		sprite.flipX = true;
        	}
        	else if (transform.position.x > moveSpot.x)
        	{
            		sprite.flipX = false;
        	}

        	if (distance < 0.2f) // Chegou
        	{
            		if (waitTime <= 0)
            		{
                		moveSpot = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                		waitTime = startWaitTime;
            		}
            		else
            		{
                		waitTime -= Time.deltaTime;
            		}
        	}

        	animator.SetFloat("Distance", distance);
        }
    }

    IEnumerator PreventStuck(Collision2D collision)
    {
        yield return new WaitForSeconds(startWaitTime + Time.deltaTime); // Espera a animação de idle
        yield return new WaitForSeconds(5); // Conta 5 segundos

        if (collision.collider != null && collision.otherCollider != null)
        {
            if (collision.collider.IsTouching(collision.otherCollider)) // Está grudado onde colidiu ainda
            {
                Debug.Log(this.name + " got Stuck");

                Rigidbody2D rigidBody = collision.otherRigidbody;
                Vector2 direction = (rigidBody.position - collision.GetContact(0).point).normalized;

                rigidBody.AddForce(direction * 50, ForceMode2D.Impulse); // Empurrãozinho
                moveSpot = transform.position;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponent<Player>();
        if (player == null) // Não é um Player
        {
            moveSpot = transform.position; // Para no lugar
            StartCoroutine(PreventStuck(collision));
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Por algum motivo, a unidade do CircleCollider2D é 10 vezes menor
        float hitRadius = circleCollider.radius * 10;
        float offsetX = circleCollider.offset.x * 10;
        float offsetY = circleCollider.offset.y * 10;

        float drawMinX = minX + offsetX - hitRadius;
        float drawMaxX = maxX + offsetX + hitRadius;
        float drawMinY = minY + offsetY - hitRadius;
        float drawMaxY = maxY + offsetY + hitRadius;

        Vector3 topLeft = new Vector3(drawMinX, drawMaxY);
        Vector3 topRight = new Vector3(drawMaxX, drawMaxY);
        Vector3 bottomLeft = new Vector3(drawMinX, drawMinY);
        Vector3 bottomRight = new Vector3(drawMaxX, drawMinY);

        if (topLeft.y < bottomLeft.y || topLeft.x > topRight.x)
        {
            // Os valores mínimos e máximos não condizem
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.white;
        }

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topLeft, bottomLeft);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomLeft, bottomRight);
        
        
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
