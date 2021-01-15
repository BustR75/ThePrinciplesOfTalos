using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DestroyAfterPlay : MonoBehaviour
{
    public float time;
    void Update()
    {
        if (time <=0)
        {
            Destroy(gameObject);
        }
        time -= Time.deltaTime;
    }
}
