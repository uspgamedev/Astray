using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum towerShootType { ON_SIGHT, CONTINUOUS, ACTIVATE }

public class Tower : MonoBehaviour
{
    public towerShootType shootType;
    public GameObject missilePreFab;
    public BoxCollider2D detectionArea;
    public Transform firePoint;
    public float shootCooldown;

    private float lastFire;
    private bool isActive = false;

    private LayerMask player;
    private ContactFilter2D playerFilter;
    private Collider2D[] playerDetected = new Collider2D[1];

    // Start is called before the first frame update
    void Start()
    {
        lastFire = -shootCooldown;

        player = LayerMask.GetMask("Player");
        playerFilter.SetLayerMask(player);
        playerFilter.useTriggers = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > lastFire + shootCooldown)
        {
            switch (shootType)
            {
                case towerShootType.CONTINUOUS:
                    Instantiate(missilePreFab, firePoint.position, firePoint.rotation);
                    lastFire = Time.time;
                    break;

                case towerShootType.ON_SIGHT:
                    if (detectionArea.OverlapCollider(playerFilter, playerDetected) == 1)
                    {
                        Instantiate(missilePreFab, firePoint.position, firePoint.rotation);
                        lastFire = Time.time;
                    }
                    break;

                case towerShootType.ACTIVATE:
                    if (isActive)
                    {
                        Instantiate(missilePreFab, firePoint.position, firePoint.rotation);
                        lastFire = Time.time;
                    }
                    else if (detectionArea.OverlapCollider(playerFilter, playerDetected) == 1)
                    {
                        isActive = true;
                    }
                    break;

                default:
                    Debug.LogError("Error on Tower Shoot Type");
                    break;
            }
        }  
    }
}
