using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*--------------------------------------------------------------------------------------------*/
//                *I used a version of a if statement many times to reduce code               //
//                          [Condition] ? [true] : [false]                                    //
/*--------------------------------------------------------------------------------------------*/

//Require Components used
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CapsuleCollider2D))]

public class PlayerController : MonoBehaviour
{
    public static PlayerController _instance;
    public Sprite[] walkCycle;
    public Sprite[] walkCycleCarry;
    public Rigidbody2D Rigidbody;
    public SpriteRenderer Renderer;
    public AudioSource Sound;
    public float MaxSpeed;
    public float Speed;
    public float JumpPower;
    public bool Grounded;
    public Grabable grabbed;
    public Transform grabPoint;
    private Vector3 originalGrabPoint;
    private Vector3 flipedGrabPoint;
    public bool allowMovement = true;

    void Start()
    {
        //make it so other objects can get the player
        _instance = this;
        
        //Set conponents if nothing was specified
        if (Rigidbody == null)
        {
            Rigidbody = GetComponent<Rigidbody2D>();
        }
        if (Renderer == null)
        {
            Renderer = GetComponent<SpriteRenderer>();
        }
        
        //can break if one is not set
        originalGrabPoint = grabPoint.localPosition;
        //flip it now so that it can be used later easily
        flipedGrabPoint = new Vector3(-originalGrabPoint.x, originalGrabPoint.y, originalGrabPoint.z);
        try
        {
            //create birds that have been gathered
            foreach (Color b in SceneManager._instance.Birds)
            {
                Bird b2 = Instantiate(SceneManager._instance.Bird, transform.position, transform.rotation).GetComponent<Bird>();
                b2.BirdColor = b;
                b2.Smoothing = Random.Range(2f, 15f);
                b2.Following = transform.Find("BirdPoint");
            }
        }
        catch { }
        // can mess up if _instance is null or Birds is null
        // only happens when restarting game
    }// start
    
    public void TryInteract()
    {
        //if nothing is grabbed try to grab what is infront of player
        if (grabbed == null)
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(grabPoint.position.x, grabPoint.position.y-.4f), Renderer.flipX ? Vector2.left : Vector2.right, .5f);
            if (hit.rigidbody != null && hit.rigidbody.GetComponent<Grabable>() != null)
            {
                grabbed = hit.rigidbody.GetComponent<Grabable>();
                grabbed.transform.SetParent(grabPoint);
                grabbed.transform.position = grabPoint.position;
                grabbed.transform.rotation = Quaternion.identity;
                grabbed.GetComponent<Rigidbody2D>().simulated = false;
                grabbed.OnGrab();
            }
        }
        // otherwise drop what is grabbed
        else
        {
            grabbed.transform.SetParent(null);
            grabbed.GetComponent<Rigidbody2D>().simulated = true;
            grabbed.OnDrop();
            grabbed = null;
        }
    } // TryInteract
    
    void Update()
    {
        //if not gameover
        if (allowMovement)
        {
            // if falling can stop double jumps
            if (Rigidbody.velocity.y < 0)
                Grounded = true;
                //Left Shift
            if (Input.GetButtonDown("Fire3"))
                TryInteract();
                // A/D,left/right
            if (Input.GetAxis("Horizontal") != 0)
            {
                //set direction
                if (Rigidbody.velocity.x < 0 || Input.GetAxis("Horizontal") < 0)
                {
                    Renderer.flipX = true;
                    grabPoint.localPosition = flipedGrabPoint;
                    if (grabbed != null)
                        grabbed.spriteRenderer.flipX = true;
                }
                else
                {
                    Renderer.flipX = false;
                    grabPoint.localPosition = originalGrabPoint;
                    if (grabbed != null)
                        grabbed.spriteRenderer.flipX = false;
                }
                //Apply speed
                Rigidbody.AddForce(new Vector2(Input.GetAxis("Horizontal") * Speed * Time.deltaTime, 0), ForceMode2D.Impulse);
                //Clamp speed to max
                Rigidbody.velocity = new Vector2(Mathf.Clamp(Rigidbody.velocity.x, -MaxSpeed, MaxSpeed), Rigidbody.velocity.y);
                //Set current sprite based on location 
                //*Changes depending on if something is grabbed
                Renderer.sprite = grabbed == null ? walkCycle[Mathf.Abs((int)(transform.position.x * 5) % (walkCycle.Length - 1))] : walkCycleCarry[Mathf.Abs((int)(transform.position.x * 5) % (walkCycleCarry.Length - 1))];
                //Only play walking if on the ground
                if (Grounded)
                    Sound.enabled = true;
            }
            else
            {
                //disable sound when not walking
                Sound.enabled = false;
                // set sprite to standing
                Renderer.sprite = grabbed == null ? walkCycle[0] : walkCycleCarry[0];
            }
        }
    }// update
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        //handle jumping
        if ((Input.GetAxis("Vertical") > 0 || Input.GetButton("Jump")) && allowMovement)
        {
            //checks all interacted objects
            for(int i = 0; i < collision.contactCount; i++)
            {
                //if on top of object
                //.5f incase the object is round 
                if (collision.GetContact(i).normal.y>.5f)
                {
                    Grounded = false;
                    //apply jump
                    Rigidbody.AddForce(new Vector2(0, JumpPower), ForceMode2D.Impulse);
                    break;
                }
            }
            
        }
    }// oncollisionstay   
}
