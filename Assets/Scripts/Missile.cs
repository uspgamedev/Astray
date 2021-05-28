using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public float missileSpeed = 20f;
    public int missileDamage = 5;
    public GameObject explodeEffect;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody.velocity = transform.right * missileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<AreaCamera>() == null &&
            collider.CompareTag("Tower") == false ) // Não é uma câmera nem a torre
        {
            Player player = collider.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(missileDamage);
            }
            Debug.Log("Missile hit " + collider.name);
            Destroy(gameObject);

            Instantiate(explodeEffect, transform.position, transform.rotation);
        }
    }
}
