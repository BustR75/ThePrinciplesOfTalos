using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Extentions;

namespace Apotheosis.Cheat
{
    // checks for cheat codes
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
        // gets all cheats in anything loaded into the game
        public void ReloadCheats()
        {
            // remove so no dupes or null values due to hot loading
            cheats.Clear();
            foreach(Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach(Type t in a.GetTypes().Where(x=> x.BaseType == typeof(CheatCode) && !x.IsAbstract))
                {
                    //create a instance of the cheat
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
    // only used in the editor
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
    // makes birds rainbow
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
    // add trail renderer to plauer
    public class Tail : CheatCode
    {
        public override void Method()
        {
            TrailRenderer r = SceneManager.robot.GetAddComponent<TrailRenderer>();
            r.widthCurve = new AnimationCurve(new Keyframe(0, .1f),new Keyframe(.1f, .5f), new Keyframe(1, 0f));

            // create rainbow gradient
            r.colorGradient = new Gradient()
            {
                colorKeys = new GradientColorKey[] 
                {
                    new GradientColorKey(new Color(1, 0, 0), 0),
                    new GradientColorKey(new Color(1, 0.1764706f, 0), .154f),
                    new GradientColorKey(new Color(0.8627451f, 1, 0), .344f),
                    new GradientColorKey(new Color(0.3607843f, 1, 0), .395f),
                    new GradientColorKey(new Color(0, 1, 0.7137255f), .641f),
                    new GradientColorKey(new Color(0, 0.6588235f, 1), .691f),
                    new GradientColorKey(new Color(0.8627451f, 0, 1), .986f),
                    new GradientColorKey(new Color(1, 0, 1), 1f)
                }
            };
            // how long in memory a position is
            r.time = 1f;
            // make sure that trail isn't pink
            r.material = new Material(Shader.Find("Sprites/Default"));
        }
    }
    //fly around
    //insperation from idclip from doom
    // currently create null reference exception Fixed
    public class Clip : CheatCode
    {
        bool enabled = false;
        public override void Method()
        {
            enabled = !enabled;
            if (enabled)
                PlayerController._instance.StartCoroutine(Flight());
        }
        IEnumerator Flight()
        {
            PlayerController._instance.Rigidbody.simulated = false;
            while (enabled)
            {
                PlayerController._instance.transform.position += new Vector3(Input.GetAxis("Horizontal") * PlayerController._instance.Speed * Time.deltaTime, Input.GetAxis("Vertical") * PlayerController._instance.Speed * Time.deltaTime, 0);
                yield return new WaitForEndOfFrame();
            }
            PlayerController._instance.Rigidbody.simulated = true;
            yield break;
        }
    }
}
