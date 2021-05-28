using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPreFab;

    public float bulletSpeed = 20f;
    public AudioSource bulletSound;
    private Vector3 offset = new Vector3(0f, -0.7f, 0f);

    public float shootCooldown = 0.2f;
    private float lastFire;
    private GameManager manager;

    void Start()
    {
        manager = FindObjectOfType<GameManager>();
    }

    private void Shoot(float horizontal, float vertical)
    {
        int bulletX;
        int bulletY;

        if (horizontal < 0)
        {
            bulletX = -1;
        }
        else if (horizontal > 0)
        {
            bulletX = 1;
        }
        else
        {
            bulletX = 0;
        }

        if (vertical < 0)
        {
            bulletY = -1;
        }
        else if (vertical > 0)
        {
            bulletY = 1;
        }
        else
        {
            bulletY = 0;
        }

        Vector2 velocity = new Vector2(bulletX, bulletY).normalized;

        GameObject bullet = Instantiate(bulletPreFab, transform.position + offset, transform.rotation);
        bullet.transform.rotation = Quaternion.FromToRotation(Vector2.right, velocity);
        bullet.GetComponent<Rigidbody2D>().velocity = velocity * bulletSpeed;
        bulletSound.Play();
        lastFire = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float shootHorizontal = Input.GetAxis("ShootHorizontal");
        float shootVertical = Input.GetAxis("ShootVertical");

        if (Time.time > lastFire + shootCooldown && !manager.gameIsPaused)
        {
            if (shootHorizontal != 0 || shootVertical != 0)
            {
                Shoot(shootHorizontal, shootVertical);
            }
        }
    }
}