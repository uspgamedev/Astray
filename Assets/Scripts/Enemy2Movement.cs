using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Movement : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Animator animator;
    public float speed;    
    public float stoppingDistance;
    private Transform target;
    public Vector3 Posicao;

    private GameManager manager;

    // Ativacao do personagem
    public float startMovementMinX;
    public float startMovementMaxX;
    public float startMovementMinY;
    public float startMovementMaxY;
    public bool startMovement = false;

    void Start()
    {
        manager = FindObjectOfType<GameManager>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();  
    }

    public void move()
    {
        startMovement = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (manager != null)
        {
            if (manager.playerAlive == false)
            {
                GetComponent<Enemy2Movement>().enabled = false;
            }
        }
    }

    // Called after Update
    void LateUpdate()
    {
    	if (target.position.x > startMovementMinX && target.position.x < startMovementMaxX && target.position.y > startMovementMinY && target.position.y < startMovementMaxY){
            animator.SetTrigger("WakeUp");
    	}

    	if (startMovement){

            if (transform.position.x < target.position.x)
            {
                sprite.flipX = true;
            }
            else if (transform.position.x > target.position.x)
            {
                sprite.flipX = false;
            }

            if (Vector2.Distance(transform.position, target.position) < stoppingDistance)
        	{
           		transform.position = Vector2.MoveTowards(transform.position, target.position + Posicao, speed * Time.deltaTime);
        	}
        }       
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
