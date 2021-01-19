using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform Following;
    public float Smoothing;
    private void Start()
    {
        if(Following == null)
        {
            // remove if isn't following
            Destroy(this);
        }
    }
    public void Update()
    {
        // if smoothing is less than or equal to zero then default to 1f
        if (Smoothing <= 0)
            transform.position = new Vector3(Following.position.x,Following.position.y,transform.position.z);
        else
        // apply position by smoothing
        // add 1 because anything devided by 1 is its self
        transform.position += new Vector3((Following.position.x-transform.position.x)/ (Smoothing + 1), (Following.position.y - transform.position.y) / (Smoothing+1), 0);
    }
}
