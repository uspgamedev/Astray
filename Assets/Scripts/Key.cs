using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum keyType { COMMON, BOSS }

public class Key : MonoBehaviour
{
    public keyType type;
    public Player player;
    public KeyUI keyUI;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject.GetComponent<Player>();
            player.keySound.Play();

            player.keys.Add(this);

            if (type == keyType.COMMON)
            {
                player.commonKeysAmount += 1;
                player.keyUI.ShowKeysAmount();
            } else if (type == keyType.BOSS)
            {
                keyUI.ShowBossKey();
            }

            this.gameObject.SetActive(false);
        }
    }

}
