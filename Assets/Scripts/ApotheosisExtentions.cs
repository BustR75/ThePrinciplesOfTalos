using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Extentions
{
    public static class ApotheosisExtentions
    {
        public static T GetAddComponent<T>(this Component instance) where T : Component
        {
            if (!instance.GetComponent<T>())
            {
                return instance.gameObject.AddComponent<T>();
            }
            return instance.GetComponent<T>();
        }
    }
}
