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
            new SingleButton("Main", "Start Game", .5f, 2, delegate
            {
                SceneManager._instance.LoadLevel();
                SceneManager._instance.Birds.Clear();
            });
            Destroy(this);
        }
        time -= Time.deltaTime;
    }
}
