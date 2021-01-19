using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//set required components
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PressurePlate : MonoBehaviour
{
    public List<Gate> Interact = new List<Gate>();
    public Sprite[] plates;
    public BoxCollider2D BoxCollider;
    SpriteRenderer renderer;
    AudioSource sound;
    public Vector2 size;
    void Start()
    {
        //set required components
        sound = GetComponent<AudioSource>();
        renderer = GetComponent<SpriteRenderer>();
    }
    //Works on non player objects
    private void OnTriggerEnter(Collider other)
    {
        
        Press(true);
        sound.enabled = true;
    }
    // only works on player for some reason
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Press(true);
        sound.enabled = true;
    }
    // works on player and objects
    private void OnTriggerExit2D(Collider2D collision)
    {
        Press(false);
        sound.enabled = false;
    }
    // changed whether things are pressed or not
    // kept alive by gate code
    void Press(bool state)
    {
        //set the sprite of plate
        renderer.sprite = plates[state ? 1 : 0];
        BoxCollider.size = new Vector2(1, state ? 0.1561432f : 0.25f);
        BoxCollider.offset = new Vector2(0, state ? -0.04692841f : 0);
        //set the objects
        foreach (var v in Interact)
        {
            v.Interact(state, gameObject);
        }
    }
}
