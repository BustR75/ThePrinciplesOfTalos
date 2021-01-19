using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    // allows for logging
    public class Hud : MonoBehaviour
    {
        public static Hud Instance;
        List<string> Log = new List<string>();
        Text Info;
        private void Start()
        {
            Info = GetComponent<Text>();
            Instance = this;
        }
        public void InGameLog(string log,float time = 5f)
        {
            StartCoroutine(LogTime(log, time));
        }
        IEnumerator LogTime(string log, float time)
        {
            Log.Add(log);
            Info.text = string.Join(Environment.NewLine, Log);
            yield return new WaitForSecondsRealtime(time);
            Log.Remove(log);
            Info.text = string.Join(Environment.NewLine, Log);
            yield break;
        }
    }
}
