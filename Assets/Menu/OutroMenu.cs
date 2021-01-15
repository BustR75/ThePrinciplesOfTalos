using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Menu;

public class OutroMenu : MonoBehaviour
{
    public Sprite image;
    float time = .5f;
    private void Update()
    {
        if (time <= 0)
        {
            new Image("Main", image, .5f, 1, 700, 350);
            new Label("Main", "<color=green>You Finished</color>", .5f, 4f, 60);
            new SingleButton("Main", "Replay Game", .5f, 2, delegate
            {
                SceneManager._instance.level = 0;
                SceneManager._instance.LoadLevel();
                SceneManager._instance.Birds.Clear();
            });
            Destroy(this);
        }
        time -= Time.deltaTime;
    }
}

