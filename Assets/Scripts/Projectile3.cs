using UnityEngine;

public class Projectile3 : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public int bulletDamage = 10;
    public GameObject impactEffect;
    public SpriteRenderer sprite;
    public float speed;
    public float pushback;
    private Transform player;
    private Vector3 target;
    private Vector3 inicial;
    public Vector3 posicao;
    
    // Start is called before the first frame update
    void Start()
    {
    	player = GameObject.FindGameObjectWithTag("Player").transform;
    	target = new Vector2(player.position.x,player.position.y);
    	inicial =  transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, (target + transform.position - inicial + posicao),speed*Time.deltaTime);
        if (!sprite.isVisible)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<AreaCamera>() == null &&
            collider.CompareTag("Water") == false &&
            collider.GetComponent<Enemy3>() == null &&
            collider.GetComponent<Enemy4>() == null ) // Adição
        {
            Debug.Log("Bullet hit " + collider.name);

            Enemy1 enemy1 = collider.GetComponent<Enemy1>();
            Enemy2 enemy2 = collider.GetComponent<Enemy2>();
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
            }else if(player != null){
                player.TakeDamage(bulletDamage);
            }

            Destroy(gameObject);

            Instantiate(impactEffect, transform.position, transform.rotation);
        }
    }
}
