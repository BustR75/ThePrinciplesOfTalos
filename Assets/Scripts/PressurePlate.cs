using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        sound = GetComponent<AudioSource>();
        renderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Press(true);
        sound.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Press(true);
        sound.enabled = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Press(false);
        sound.enabled = false;
    }
    void Press(bool state)
    {
        renderer.sprite = plates[state ? 1 : 0];
        BoxCollider.size = new Vector2(1, state ? 0.1561432f : 0.25f);
        BoxCollider.offset = new Vector2(0, state ? -0.04692841f : 0);
        foreach (var v in Interact)
        {
            v.Interact(state, gameObject);
        }
    }
}
