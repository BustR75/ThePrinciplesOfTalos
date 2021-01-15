using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class MainMenu : MonoBehaviour
    {
        public Sprite image;
        public NestedMenu Options;
#if UNITY_EDITOR
        public static float timescale = 1f;
#endif
        private void Start()
        {
            new Image("Main", image, .5f, 0, 350, 175);
            new SingleButton("Main", "Resume", .5f, 1, delegate
                {
                    MenuManager.instance.ToggleMenu(false);
                });
            new SingleButton("Main", "Exit Game", 0, 2, delegate
                {
#if !UNITY_EDITOR
                    Application.Quit(0);
#endif
                });
            new SingleButton("Main", "Restart Game", 1, 2, delegate
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                SceneManager._instance.level = 0;
                Time.timeScale = 1f;
            });
            new SingleButton("Main", "Restart Level", 1, 3, delegate
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, UnityEngine.SceneManagement.LoadSceneMode.Single);
                    Time.timeScale = 1f;
                });
#if UNITY_EDITOR
            new NestedMenu("Main", "Debug", .5f, 5);
            new Slider("Debug", "TimeScale", 0, 0, false, 1, .001f, 2f, delegate (float f)
                    {
                        timescale = f;
                    });
            new NestedMenu("Debug", "Summon", 1, 0);
            new SingleButton("Summon", "Cube", 0, 0, delegate
                {
                    GameObject.Instantiate(SceneManager._instance.Cube, SceneManager.robot.transform.position + SceneManager.robot.transform.forward, Quaternion.identity);
                });
            new SingleButton("Summon", "Bird", 1, 0, delegate
            {
                Bird b = GameObject.Instantiate(SceneManager._instance.Bird, SceneManager.robot.transform.position + SceneManager.robot.transform.forward, Quaternion.identity).GetComponent<Bird>();
                b.Following = SceneManager.robot.transform.Find("BirdPoint");
                b.Smoothing = UnityEngine.Random.Range(3f, 25f);
            });
            new SingleButton("Summon", "Trampoline", 0, 1, delegate
            {
                GameObject.Instantiate(SceneManager._instance.Trampoline, SceneManager.robot.transform.position + SceneManager.robot.transform.forward, Quaternion.identity);
            });
            new SingleButton("Summon", "Jammer", 1, 1, delegate
            {
                GameObject.Instantiate(SceneManager._instance.Jammer, SceneManager.robot.transform.position + SceneManager.robot.transform.forward, Quaternion.identity);
            });
#endif
        }
    }
}
