using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Menu;

namespace Assets.Menu
{
    public class GameOver : MonoBehaviour
    {
        public Sprite Image;
        public void Start()
        {
            new Image("GameOver", Image, .5f, 0, 350, 175);
            new Label("GameOver", "<color=red>GameOver</color>", .5f, 1,50);
            new SingleButton("GameOver","Restart",.5f,2,delegate
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, LoadSceneMode.Single);
                Time.timeScale = 1f;
            });
        }
    }
}
