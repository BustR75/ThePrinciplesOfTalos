using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jammer : Grabable
{
    bool grabbed = false;
    Gate interacting;
    Bomb interactingbomb;
    public GameObject Aura;
    public SpriteRenderer AuraSprite;
    Vector3 opos;
    Vector3 fpos;
    private void Start()
    {
        base.Start();
        opos = Aura.transform.localPosition;
        fpos = new Vector3(opos.x*-1,opos.y,opos.z);
        AuraSprite = Aura.GetComponent<SpriteRenderer>();
    }
    public override void OnDrop()
    {
        grabbed = false;
        // scan direction and grabs hits
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x-(spriteRenderer.flipX ? 0.43f : -0.43f),transform.position.y - 0.11f), spriteRenderer.flipX ? Vector2.left : Vector2.right);
        // if it found something
        if(hit.collider != null)
        {
            // if thing is gate
            if (hit.collider.GetComponent<Gate>())
            {
                interacting = hit.collider.GetComponent<Gate>();
                interacting.Interact(true, gameObject);
                Aura.SetActive(true);
                Aura.transform.localPosition = spriteRenderer.flipX ? fpos : opos;
                AuraSprite.flipX = spriteRenderer.flipX;

            }
            // if thing is bomb
            else if (hit.collider.GetComponent<Bomb>())
            {
                interactingbomb = hit.collider.GetComponent<Bomb>();
                interactingbomb.Jam(this,true);
                Aura.SetActive(true);
                Aura.transform.localPosition = spriteRenderer.flipX ? fpos : opos;
                AuraSprite.flipX = spriteRenderer.flipX;
            }
        }
    }

    public override void OnGrab()
    {
        grabbed = true;
        // ? incase interacting is null
        interacting?.Interact(false, gameObject);
        //cannot set a bool with same method
        if(interactingbomb != null)
                interactingbomb.Jam(this,false);
        // remove references
        interactingbomb = null;
        interacting = null;
        
        Aura.SetActive(false);
    }
}
