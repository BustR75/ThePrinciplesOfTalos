using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.Scripts
{
    public class Background : MonoBehaviour
    {
        private void Update()
        {
            // apply paralax effect
            transform.position = new Vector3(Camera.main.transform.position.x*.9f, Camera.main.transform.position.y*.9f, 10);
        }
    }
}
