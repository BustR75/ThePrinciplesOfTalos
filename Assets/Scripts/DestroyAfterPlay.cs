using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
// used for particle systems so that less memory is used needlessly
public class DestroyAfterPlay : MonoBehaviour
{
    public float time;
    void Update()
    {
        if (time <=0)
        {
            Destroy(gameObject);
        }
        //count down
        time -= Time.deltaTime;
    }
}
