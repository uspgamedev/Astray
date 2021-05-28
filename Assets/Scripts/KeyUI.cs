using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoBehaviour
{
    public Text keysAmount;
    [HideInInspector]public Player player;
    public GameObject bossKey;

    // Start is called before the first frame update

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void ShowKeysAmount()
    {
        keysAmount.text = player.commonKeysAmount.ToString();
    }

    public void ShowBossKey()
    {
        bossKey.SetActive(true);
    }

    public void HideBossKey()
    {
        bossKey.SetActive(false);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
