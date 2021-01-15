using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Extentions;

namespace Apotheosis.Cheat
{
    public class CheatCodeManager : MonoBehaviour
    {
        public static CheatCodeManager _instance;
        public string starter = "ap";
        public string currentString = "`";
        public Dictionary<string, CheatCode> cheats = new Dictionary<string, CheatCode>();
        private void Start()
        {
            _instance = this;
            ReloadCheats();
        }
        public void ReloadCheats()
        {
            cheats.Clear();
            foreach(Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach(Type t in a.GetTypes().Where(x=> x.BaseType == typeof(CheatCode) && !x.IsAbstract))
                {
                    cheats.Add(t.Name.ToLower(),(Activator.CreateInstance(t) as CheatCode));
                }
            }
        }
        void Update()
        {
            if (Input.anyKeyDown)
            {
                currentString += Input.inputString;
                try
                {
                    if (currentString.Substring(currentString.Length - starter.Length) == starter.ToString())
                    {
                        currentString = "";
                    }
                }
                catch(ArgumentOutOfRangeException)
                {
                    //if less than starter length
                }
                foreach (var cheat in cheats)
                {
                    if (currentString == cheat.Key)
                    {
                        cheat.Value.Method();
                        currentString = "`";
                    }
                }
            }
        }
    }
    public abstract class CheatCode
    {
        public abstract void Method();
    }
#if UNITY_EDITOR
    public class Help : CheatCode
    {
        public override void Method()
        {
            foreach (var c in CheatCodeManager._instance.cheats)
            {
                Debug.Log(c.Key);
                Console.Write(c.Key);
                Menu.Hud.Instance.InGameLog(c.Key, 5f);
            }
        }
    }
#endif
    public class CreateCube : CheatCode
    {
        public override void Method()
        {
            GameObject.Instantiate(SceneManager._instance.Cube, SceneManager.robot.transform.position + SceneManager.robot.transform.forward, Quaternion.identity);
        }
    }
    public class CreateTrampoline : CheatCode
    {
        public override void Method()
        {
            GameObject.Instantiate(SceneManager._instance.Trampoline, SceneManager.robot.transform.position + SceneManager.robot.transform.forward, Quaternion.identity);
        }
    }
    public class CreateJammer : CheatCode
    {
        public override void Method()
        {
            GameObject.Instantiate(SceneManager._instance.Jammer, SceneManager.robot.transform.position + SceneManager.robot.transform.forward, Quaternion.identity);
        }
    }
    public class CreateBird : CheatCode
    {
        public override void Method()
        {
           Bird b = GameObject.Instantiate(SceneManager._instance.Bird, SceneManager.robot.transform.position + SceneManager.robot.transform.forward, Quaternion.identity).GetComponent<Bird>();
            b.Following = SceneManager.robot.transform.Find("BirdPoint");
            b.Smoothing = UnityEngine.Random.Range(3f, 25f);
            b.RandomColor = true;
        }
    }
    public class Jeb_ : CheatCode
    {
        public override void Method()
        {
            foreach (Bird b in Resources.FindObjectsOfTypeAll<Bird>())
            {
                b.Rainbow = true;
            }
        }
    }
    public class Tail : CheatCode
    {
        public override void Method()
        {
            TrailRenderer r = SceneManager.robot.GetAddComponent<TrailRenderer>();
            r.widthCurve = new AnimationCurve(new Keyframe(0, .1f),new Keyframe(.1f, .5f), new Keyframe(1, 0f));


            r.colorGradient = new Gradient()
            {
                colorKeys = new GradientColorKey[] 
                {
                    new GradientColorKey(Color.red, 0),
                    new GradientColorKey(new Color(1, 0.1764706f, 0), .154f),
                    new GradientColorKey(new Color(0.8627451f, 1, 0), .344f),
                    new GradientColorKey(new Color(0.3607843f, 1, 0), .395f),
                    new GradientColorKey(new Color(0, 1, 0.7137255f), .641f),
                    new GradientColorKey(new Color(0, 0.6588235f, 1), .691f),
                    new GradientColorKey(new Color(0.8627451f, 0, 1), .986f),
                    new GradientColorKey(new Color(1, 0, 1), 1f)
                }
            };
            r.time = 1f;
            r.material = new Material(Shader.Find("Sprites/Default"));
        }
    }
    public class Clip : CheatCode
    {
        bool enabled = false;
        public override void Method()
        {
            enabled = !enabled;
            if (enabled)
                CheatCodeManager._instance.StartCoroutine(Flight());
        }
        IEnumerator Flight()
        {
            SceneManager.robot.Rigidbody.simulated = false;
            while (enabled)
            {
                SceneManager.robot.transform.position += new Vector3(Input.GetAxis("Horizontal") * PlayerController._instance.Speed * Time.deltaTime, Input.GetAxis("Vertical") * PlayerController._instance.Speed * Time.deltaTime, 0);
                yield return new WaitForEndOfFrame();
            }
            SceneManager.robot.Rigidbody.simulated = true;
            yield break;
        }
    }
}
