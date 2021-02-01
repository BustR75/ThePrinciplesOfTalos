using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Menu;

public class IntroMenu : MonoBehaviour
{
    public Sprite image;
    float time = .5f;
    private void Update()
    {
        if (time <= 0)
        {
            new Image("Main", image, .5f, 1, 700, 350);
            new SingleButton("Main", "Start Game", 0f, 2, delegate
            {
                SceneManager._instance.LoadLevel();
                SceneManager._instance.Birds.Clear();
            });
            Destroy(this);
            new NestedMenu("Main", "Info", 1, 2);
            Label info = new Label("Info",
                "In this game you <color=lightblue><b>Move</b></color> with the <color=#FFAAAA><b>A D</b></color> or <color=#FFAAAA><b>Left Right</b></color> Keys\n" +
                "<color=#FFAAAA><b>Space</b></color>, <color=#FFAAAA><b>W</b></color>, or <color=#FFAAAA><b>Up</b></color> to <color=lightblue><b>Jump</b></color>\n" +
                "<color=#FFAAAA><b>Left Shift</b></color> to <color=lightblue><b>Grab</b></color>\n" +
                "Objects in this game are:\n" +
                "<color=#FFAAAA><b>Cubes</b></color> which can be <color=lightblue><b>Grabbed</b></color> and <color=lightblue><b>Stood</b></color> on\n" +
                "<color=#FFAAAA><b>Jammers</b></color> can be used to <color=lightblue><b>Jam</b></color> <color=#FFAAAA><b>Gates</b></color> and <color=#FFAAAA><b>Bombs</b></color>; they can also be <color=lightblue><b>Stood</b></color> on\n" +
                "<color=#FFAAAA><b>Gates</b></color> <color=lightblue><b>Stop</b></color> the <color=#FFAAAA><b>Player</b></color> and can be <color=lightblue><b>Opened</b></color> with <color=#FFAAAA><b>Pressure Plates</b></color> and <color=#FFAAAA><b>Jammers</b></color>\n" +
                "<color=#FFAAAA><b>Pressure Plates</b></color> <color=lightblue><b>Open</b></color> <color=#FFAAAA><b>Gates</b></color> when <color=lightblue><b>Stood</b></color> on by <color=#FFAAAA><b>Objects</b></color> and <color=#FFAAAA><b>Player</b></color>\n" +
                "<color=#FFAAAA><b>Bombs</b></color> <color=lightblue><b>Follow</b></color> a set <color=#FFAAAA><b>Path</b></color> and will <color=lightblue><b>Fly</b></color> at the <color=#FFAAAA><b>Player</b></color> when <color=lightblue><b>Spotted</b></color>\n" +
                "<color=#FFAAAA><b>Birds</b></color> are the object of the game <color=lightblue><b>Reach</b></color> them to get to the <color=#FFAAAA><b>Next Level</b></color>.\n" +
                "When <color=lightblue><b>Collected</b></color> they will <color=lightblue><b>Follow</b></color> the <color=#FFAAAA><b>Player</b></color>"
                , .5f, 5,18);
            RectTransform rect = info.gObject.GetComponent<RectTransform>();
            rect.localPosition = new Vector3(0, 40, 0);
            rect.sizeDelta = new Vector2(500, 300);
            info.text.alignment = TextAnchor.UpperLeft;
        }
        time -= Time.deltaTime;
    }
}
