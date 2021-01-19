using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Extentions
{
    // Apotheosis is my calling card* for things i make
    public static class ApotheosisExtentions
    {
        // is used a few times to get or add a component
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
