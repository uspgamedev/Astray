using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public Boss bossLife;
    public BossAttack attack;
    public CircleCollider2D circleCollider;
    public SpriteRenderer sprite;
    public Animator animator;
    public Transform roomCamera;

    public float speedStage1 = 3.5f;
    public float speedStage2 = 4f;
    public float startWaitTime = 2;
    private float waitTime;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    private Vector2 moveSpot;
    private float moveX;

    public float offsetArm = 1.6f;
    public float offsetPunchX = 3;
    public float offsetPunchY = 1;
    private Transform target;

    public float roomSpaceX = 12f;
    public float roomSpaceY = 7.75f;
    private float roomMaxX;
    private float roomMinX;
    private float roomMaxY;
    private float roomMinY;

    private bool rotated = false;

    void Start()
    {
        waitTime = startWaitTime;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        moveX = Random.Range(minX, maxX);

        roomMaxX = roomCamera.position.x - circleCollider.offset.x - circleCollider.radius + roomSpaceX;
        roomMinX = roomCamera.position.x - circleCollider.offset.x + circleCollider.radius - roomSpaceX;
        roomMaxY = roomCamera.position.y - circleCollider.offset.y - circleCollider.radius + roomSpaceY;
        roomMinY = roomCamera.position.y - circleCollider.offset.y + circleCollider.radius - roomSpaceY;
    }

    void LateUpdate()
    {
        if (bossLife.isVulnerable == true)
        {
            if (bossLife.isHalfLife == true)
            {
                MovementStage2();
            }
            else
            {
                MovementStage1();
            }
        }

        if (bossLife.isImovable == false) // Rotacionar
        {
            if (transform.position.x > target.position.x) // Boss à direita do Player
            {
                if (rotated == false)
                {
                    transform.RotateAround(transform.position, Vector3.up, 180);
                    rotated = true;
                }
            }
            else
            {
                if (rotated == true)
                {
                    transform.RotateAround(transform.position, Vector3.up, 180);
                    rotated = false;
                    offsetPunchX *= -1;
                }
            }
        }
    }

    void MovementStage1()
    {
        float moveY;
        if (target.position.y > maxY + offsetArm)
        {
            moveY = maxY;
        }
        else if (target.position.y < minY + offsetArm)
        {
            moveY = minY;
        }
        else
        {
            moveY = target.position.y - offsetArm;
        }

        moveSpot = new Vector2(moveX, moveY); // Y do Player, X dentro das coordenadas
           
        transform.position = Vector2.MoveTowards(transform.position, moveSpot, speedStage1 * Time.deltaTime);

       if (waitTime <= 0)
       {
            moveX = Random.Range(minX, maxX);
            waitTime = startWaitTime;
       }
       else
       {
            waitTime -= Time.deltaTime;
       }
    }

    void MovementStage2()
    {
        Vector2 punchTarget = new Vector2(target.position.x + offsetPunchX, target.position.y + offsetPunchY);

        if (Vector2.Distance(transform.position, punchTarget) > 0.2f)
        {
            Vector2 moveTarget = Vector2.zero;
            moveTarget.x = Mathf.Min(Mathf.Max(roomMinX, punchTarget.x), roomMaxX);
            moveTarget.y = Mathf.Min(Mathf.Max(roomMinY, punchTarget.y), roomMaxY);

            transform.position = Vector2.MoveTowards(transform.position, moveTarget, speedStage2 * Time.deltaTime);
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

    IEnumerator PreventStuck(Collision2D collision)
    {
        yield return new WaitForSeconds(startWaitTime + Time.deltaTime); // Espera a animação de idle
        yield return new WaitForSeconds(5); // Conta 5 segundos

        if (collision.collider.IsTouching(collision.otherCollider)) // Está grudado onde colidiu ainda
        {
            Debug.Log(this.name + " got Stuck");

            Rigidbody2D rigidBody = collision.otherRigidbody;
            Vector2 direction = (rigidBody.position - collision.GetContact(0).point).normalized;

            rigidBody.AddForce(direction * 50, ForceMode2D.Impulse); // Empurrãozinho
            moveSpot = transform.position;
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
    }
}
