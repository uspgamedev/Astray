using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    public ParticleSystem particles;

    // Update is called once per frame
    void Update()
    {
        if (particles.IsAlive() == false)
        {
            Destroy(gameObject);
        }
    }
}
