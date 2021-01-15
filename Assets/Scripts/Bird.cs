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
    public float IdleLeanience;

    // Start is called before the first frame update
    void Start()
    {
        if(BirdColor.a == 0)
        {
            BirdColor.a = 255;
        }
        if (birdrenderer == null)
        {
            birdrenderer = GetComponent<SpriteRenderer>();
        }
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }
    private void OnTriggerEnter(Collider other)
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
    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (birdrenderer.color != BirdColor && !Rainbow)
        {
            birdrenderer.color = BirdColor;
        }
        if (Rainbow)
        {
            birdrenderer.color = Color.HSVToRGB(Mathf.PingPong(Time.time * .1f, 1), 1, 1);
        }
        if (RandomColor)
        {
            RandomColor = false;
            BirdColor = Color.HSVToRGB(Random.Range(0f, 1f), 1, 1);
        }
        animator.SetBool("Idle", Following.position.x - IdleLeanience <= transform.position.x && Following.position.x+ IdleLeanience >= transform.position.x && Following.position.y+ IdleLeanience >= transform.position.y && Following.position.y- IdleLeanience <= transform.position.y);
        if (Following.position.x > transform.position.x)
        {
            birdrenderer.flipX = true;
        }
        else
        {
            birdrenderer.flipX = false;
        }

    }
}
