using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public int level = 0;
    public void LoadLevel()
    {
        level++;
        UnityEngine.SceneManagement.SceneManager.LoadScene(level);
    }
    private void Start()
    {
        // remove self if there is already a scenemanager
        // mainly used when creating levels so that I can easily test them
        if (_instance != null) Destroy(gameObject);
        else
        {
            _instance = this;
            // make it so this is persistant
            DontDestroyOnLoad(gameObject);
        }
        
    }
    public static SceneManager _instance;
    // allows for ease of use for cheat codes
    public static PlayerController robot
    {
        get
        {
            return PlayerController._instance;
        }
    }
    // required for some debug and cheats
    public GameObject Cube;
    public GameObject Jammer;
    public GameObject Bird;
    public GameObject Trampoline;
    // contains the colors of the previus levels
    public List<Color> Birds = new List<Color>();
}
