using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObject : MonoBehaviour
{
    public GameObject objectToShow;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        objectToShow.SetActive(true);      
    }
}
