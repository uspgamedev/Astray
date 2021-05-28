using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vidinha : MonoBehaviour
{


    public int regeneration;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
	    player.TakeDamage(-regeneration);
            this.gameObject.SetActive(false);
        }
    }
}
