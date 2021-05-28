using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBossTheme : MonoBehaviour
{
    private void Awake()
    {
        FindObjectOfType<AudioManager>().Play("BossTheme");
    }
}
