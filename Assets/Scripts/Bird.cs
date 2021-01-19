using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]

public class Bird : Follower
{
    public SpriteRenderer birdrenderer;
    public Animator animator;
    public Color BirdColor;
    public bool Rainbow;
    public bool RandomColor;
    // if zero the bird will always be flying
    public float IdleLeanience;

    // Start is called before the first frame update
    void Start()
    {
        // make sure bird is visable
        if(BirdColor.a == 0)
        {
            BirdColor.a = 255;
        }
        // set if net specified
        if (birdrenderer == null)
        {
            birdrenderer = GetComponent<SpriteRenderer>();
        }
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }
    //only used with pedistals
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //remove collider and apply bird to player
            Destroy(GetComponent<Collider2D>());
            transform.SetParent(null);
            SceneManager._instance.Birds.Add(BirdColor);
            Following = PlayerController._instance.transform.Find("BirdPoint");
            Smoothing = Random.Range(2, 15);
            // set next level
            SceneManager._instance.LoadLevel();
        }
    }
    // kept just in case
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(GetComponent<Collider2D>());
            transform.SetParent(null);
            SceneManager._instance.Birds.Add(BirdColor);
            Following = PlayerController._instance.transform.Find("BirdPoint");
            Smoothing = Random.Range(2, 15);
            SceneManager._instance.LoadLevel();
        }
    }
    
    void Update()
    {
        //Run follower code
        base.Update();
        //set color
        if (birdrenderer.color != BirdColor && !Rainbow)
        {
            birdrenderer.color = BirdColor;
        }
        if (Rainbow) // used by jeb_ cheat
        {
            birdrenderer.color = Color.HSVToRGB(Mathf.PingPong(Time.time * .1f, 1), 1, 1);
        }
        if (RandomColor) // used when initialized
        {
            RandomColor = false;
            BirdColor = Color.HSVToRGB(Random.Range(0f, 1f), 1, 1);
        }
        // set if bird should be flying or not
        // by distance from goal
        animator.SetBool("Idle", Following.position.x - IdleLeanience <= transform.position.x && Following.position.x+ IdleLeanience >= transform.position.x && Following.position.y+ IdleLeanience >= transform.position.y && Following.position.y- IdleLeanience <= transform.position.y);
        //set direction
        birdrenderer.flipX = Following.position.x > transform.position.x;

    }
}
