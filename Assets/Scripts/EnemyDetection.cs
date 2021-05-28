using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public BoxCollider2D area;
    public GameObject key;

    private LayerMask enemies;
    private ContactFilter2D enemiesFilter;
    private List<Collider2D> enemiesDetected = new List<Collider2D>();

    private bool isObtainable;

    // Start is called before the first frame update
    void Start()
    {
        enemies = LayerMask.GetMask("Enemies");
        enemiesFilter.SetLayerMask(enemies);
        enemiesFilter.useLayerMask = true;
        enemiesFilter.useTriggers = true;

        isObtainable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isObtainable == false && area.OverlapCollider(enemiesFilter, enemiesDetected) == 0)
        {
            key.SetActive(true);
            isObtainable = true;
        }
    }
}
