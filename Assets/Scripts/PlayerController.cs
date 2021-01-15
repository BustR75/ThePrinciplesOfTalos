using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        _instance = this;
        if (Rigidbody == null)
        {
            Rigidbody = GetComponent<Rigidbody2D>();
        }
        if (Renderer == null)
        {
            Renderer = GetComponent<SpriteRenderer>();
        }
        originalGrabPoint = grabPoint.localPosition;
        flipedGrabPoint = new Vector3(-originalGrabPoint.x, originalGrabPoint.y, originalGrabPoint.z);
        try
        {
            foreach (Color b in SceneManager._instance.Birds)
            {
                Bird b2 = Instantiate(SceneManager._instance.Bird, transform.position, transform.rotation).GetComponent<Bird>();
                b2.BirdColor = b;
                b2.Smoothing = Random.Range(2f, 15f);
                b2.Following = transform.Find("BirdPoint");
            }
        }
        catch { }
    }
    public void TryInteract()
    {
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
        else
        {
            grabbed.transform.SetParent(null);
            grabbed.GetComponent<Rigidbody2D>().simulated = true;
            grabbed.OnDrop();
            grabbed = null;
        }
    }
    void Update()
    {
        if (allowMovement)
        {

            if (Rigidbody.velocity.y < 0)
                Grounded = true;
            if (Input.GetButtonDown("Fire3"))
                TryInteract();
            if (Input.GetAxis("Horizontal") != 0)
            {
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

                Rigidbody.AddForce(new Vector2(Input.GetAxis("Horizontal") * Speed * Time.deltaTime, 0), ForceMode2D.Impulse);
                Rigidbody.velocity = new Vector2(Mathf.Clamp(Rigidbody.velocity.x, -MaxSpeed, MaxSpeed), Rigidbody.velocity.y);
                Renderer.sprite = grabbed == null ? walkCycle[Mathf.Abs((int)(transform.position.x * 5) % (walkCycle.Length - 1))] : walkCycleCarry[Mathf.Abs((int)(transform.position.x * 5) % (walkCycleCarry.Length - 1))];
                if (Grounded)
                    Sound.enabled = true;
            }
            else
            {
                Sound.enabled = false;
                Renderer.sprite = grabbed == null ? walkCycle[0] : walkCycleCarry[0];
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((Input.GetAxis("Vertical") > 0 || Input.GetButton("Jump")) && allowMovement)
        {
            for(int i = 0; i < collision.contactCount; i++)
            {
                if (collision.GetContact(i).normal.y>.5f)
                {
                    Grounded = false;
                    Rigidbody.AddForce(new Vector2(0, JumpPower), ForceMode2D.Impulse);
                    break;
                }
            }
            
        }
    }
}
