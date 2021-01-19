using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(LineRenderer))]
public class Bomb : MonoBehaviour
{
    List<Jammer> Jammers = new List<Jammer>();
    float Distance;
    public float Speed;
    public GameObject ExplosionPrefab;
    Rigidbody2D Rigidbody2D;
    LineRenderer Line;
    public bool foundPlayer = false;
    bool goingLeft = false;
    
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Line = GetComponent<LineRenderer>();
        Distance = Line.GetPosition(0).x;
    }
    
    public void Jam(Jammer self, bool state)
    {
        if(state)
            Jammers.Add(self);
        else
            Jammers.Remove(self);
    }
    
    void Update()
    {
        // only update when not jammed
        // allow for redundence
        if (Jammers.Count == 0)
        {
            if (!foundPlayer)
            {
                //right scan
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + .6f, transform.position.y), Vector2.right, Distance);
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        foundPlayer = true;
                    }
                    else
                    {
                        goingLeft = true;
                    }
                }
                //left scan
                hit = Physics2D.Raycast(new Vector2(transform.position.x - .6f, transform.position.y), Vector2.left, Distance);
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        foundPlayer = true;
                    }
                    else
                    {
                        goingLeft = false;
                    }
                }
                // apply movement
                // set position so that player will slide of if on non jammed bomb
                transform.position += (goingLeft ? Vector3.left : Vector3.right) * Speed * Time.deltaTime;
            }
            else
            {
                //if the player is found fly at them with increasing speed
                Rigidbody2D.AddForce(new Vector2((PlayerController._instance.transform.position.x - transform.position.x) / 5, 0), ForceMode2D.Impulse);
                if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(PlayerController._instance.transform.position.x, PlayerController._instance.transform.position.y)) < 1)
                {
                    // gameover
                    Instantiate(ExplosionPrefab, transform.position, transform.rotation);
                    Destroy(gameObject);
                    PlayerController._instance.allowMovement = false;
                    Menu.MenuManager.instance.ToggleMenu(true);
                    Menu.MenuManager.GoToMenu("GameOver");
                    //set timescale so that explosion can play out
                    Time.timeScale = 1f;
                }
            }
        }
    }
}
