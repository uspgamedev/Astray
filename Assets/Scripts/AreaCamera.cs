using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCamera : MonoBehaviour
{
    
    void OnTriggerStay2D (Collider2D other)
    {

        if (other.CompareTag("Player"))
        {

            CameraController.instance.SetPosition(new Vector2(transform.position.x, transform.position.y));

        }
    }
}
