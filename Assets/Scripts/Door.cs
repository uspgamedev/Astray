using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public keyType type;
    public Player player;
    public KeyUI keyUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject.GetComponent<Player>();

            foreach(Key k in player.keys)
            {
                if (k.type == type)
                {
                    Destroy(this.transform.parent.gameObject);

                    if (k.type == keyType.COMMON)
                    {
                        player.commonKeysAmount -= 1;
                        player.keyUI.ShowKeysAmount();
                    } else if (type == keyType.BOSS)
                    {
                        keyUI.HideBossKey();
                    }

                    player.keys.Remove(k);
                    return;
                }            
            }
        }
    }
}
