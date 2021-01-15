using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Gate : MonoBehaviour
{
    public List<GameObject> keepactive = new List<GameObject>();
    public Sprite[] sprites;
    public SpriteRenderer renderer;
    public BoxCollider2D collider2D;
    Vector3 originalposition;
    private void Start()
    {
        originalposition = transform.localPosition;
    }
    public void Interact(bool state,GameObject self)
    {
        if (state)
            keepactive.Add(self);
        else
            keepactive.Remove(self);
        if (keepactive.Count > 0)
        {
            renderer.sprite = sprites[1];
            collider2D.size = new Vector2(0.25f, 0.0897789f);
            transform.localPosition = new Vector3(originalposition.x, originalposition.y, originalposition.z)+ transform.up * -1.423f;
        }
        else
        {
            renderer.sprite = sprites[0];
            collider2D.size = new Vector2(0.25f, 1.921875f);
            transform.localPosition = new Vector3(originalposition.x, originalposition.y, originalposition.z);
        }
    }
    void Update()
    {
        renderer.color = Color.HSVToRGB(0, 0, Mathf.PingPong(Time.time*.2f,.5f)+.5f);
    }
}