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
        if (_instance != null) Destroy(gameObject);
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
    }
    public static SceneManager _instance;
    public static PlayerController robot
    {
        get
        {
            return PlayerController._instance;
        }
    }
    public GameObject Cube;
    public GameObject Jammer;
    public GameObject Bird;
    public GameObject Trampoline;
    public List<Color> Birds = new List<Color>();
}
