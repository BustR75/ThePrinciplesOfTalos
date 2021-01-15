using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        public GameObject Single;
        public GameObject Toggle;
        public GameObject Slider;
        public GameObject Label;
        private void Start()
        {
            if (instance != null)
                Destroy(gameObject);
            instance = this;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleMenu(!CurrentMenuGameObject.activeSelf);
            }
        }
        public static MenuManager instance;
        public string CurrentMenu ="Main";
            
        GameObject CurrentMenuGameObject {
            get
            {
                return transform.Find(CurrentMenu).gameObject;
            }
        }
        public static void GoToMenu(string Name)
        {
            instance.CurrentMenuGameObject.SetActive(false);
            instance.CurrentMenu = Name;
            instance.CurrentMenuGameObject.SetActive(true);
        }
        public void ToggleMenu(bool state) 
        {
#if UNITY_EDITOR
            Time.timeScale = state ? 0f : MainMenu.timescale;
#else
            Time.timeScale = state ? 0f : 1f;
#endif
            CurrentMenuGameObject.SetActive(state);
            transform.Find("Background").gameObject.SetActive(state);
        }
    }
    public class BaseButton
    {
        public string name;
        public GameObject gObject;
        public Action Action;
        public Text text;

        public void SetPosition(float x, float y)
        {
            RectTransform rect = gObject.GetComponent<RectTransform>();
            rect.localPosition = new Vector3(-90 + (180 * x), 140 - (60 * y), 0);
        }
    }
    public class NestedMenu : BaseButton
    {
        public GameObject Menu;
        public SingleButton mainButton;
        public SingleButton backButton;
        public NestedMenu(string BaseMenu,string Name, float x, float y)
        {
            Menu = new GameObject(Name);
            Menu.transform.SetParent(MenuManager.instance.transform);
            Menu.transform.localPosition = Vector3.zero;
            Menu.SetActive(false);
            mainButton = new SingleButton(BaseMenu, Name, x, y, delegate
            {
                MenuManager.GoToMenu(Name);
            });
            backButton = new SingleButton(Name, "<color=yellow>Back</color>", 1, 5, delegate
            {
                MenuManager.GoToMenu(BaseMenu);
            });
        }
    }
    public class SingleButton : BaseButton
    {
        public SingleButton(string Menu,string Name,float x, float y, UnityAction action)
        {
            gObject = GameObject.Instantiate(MenuManager.instance.Single, MenuManager.instance.transform.Find(Menu));
            text = gObject.GetComponentInChildren<Text>();
            text.text = Name;
            SetPosition(x, y);
            gObject.GetComponent<Button>().onClick.AddListener(action);
        }
    }
    public class Toggle : BaseButton
    {
        public Toggle(string Menu, string Name, float x, float y, UnityAction<bool> action)
        {
            gObject = GameObject.Instantiate(MenuManager.instance.Toggle, MenuManager.instance.transform.Find(Menu));
            text = gObject.GetComponentInChildren<Text>();
            text.text = Name;
            SetPosition(x, y);
            gObject.GetComponent<UnityEngine.UI.Toggle>().onValueChanged.AddListener(action);
        }
    }
    public class Label : BaseButton
    {
        public Label(string Menu, string Name, float x, float y,int fontsize = 14)
        {
            gObject = GameObject.Instantiate(MenuManager.instance.Label, MenuManager.instance.transform.Find(Menu));
            text = gObject.GetComponent<Text>();
            text.text = Name;
            text.fontSize = fontsize;
            gObject.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
            SetPosition(x, y);
        }
    }
    public class Image : BaseButton
    {
        public Image(string Menu, Sprite image, float x, float y, float height, float width)
        {
            gObject = new GameObject();
            gObject.transform.SetParent(MenuManager.instance.transform.Find(Menu));
            gObject.AddComponent<UnityEngine.UI.Image>().sprite = image;
            gObject.GetComponent<RectTransform>().sizeDelta = new Vector2(height,width);
            SetPosition(x, y);
        }
    }
    public class Slider : BaseButton
    {
        public SliderMono sliderObject;
        public Slider(string Menu, string Name, float x, float y,bool WholeNumbers,float current,float min,float max, UnityAction<float> action)
        {
            gObject = GameObject.Instantiate(MenuManager.instance.Slider, MenuManager.instance.transform.Find(Menu));
            sliderObject = gObject.GetComponent<SliderMono>();
            text = sliderObject.Title;
            sliderObject.Min.text = min.ToString();
            sliderObject.Max.text = max.ToString();
            sliderObject.Current.text = current.ToString();
            text.text = Name;
            SetPosition(x, y);
            UnityEngine.UI.Slider s = gObject.GetComponent<UnityEngine.UI.Slider>();
            s.value = current;
            s.minValue = min;
            s.maxValue = max;
            s.wholeNumbers = WholeNumbers;
            s.onValueChanged.AddListener(delegate(float value)
            {
                sliderObject.Current.text = value.ToString();
            });
            s.onValueChanged.AddListener(action);
        }
    }
}
