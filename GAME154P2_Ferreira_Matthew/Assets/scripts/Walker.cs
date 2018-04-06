using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour {
    protected float speed;
    protected int health;
    protected bool isFacingLeft = false;
    Animator anim;
    Rigidbody2D rb;
    public AudioClip death;
    // Use this for initialization
    void Start()
    {
        Physics2D.IgnoreLayerCollision(13, 13, true);
        rb = GetComponent<Rigidbody2D>();
        if (!rb)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        // Used to get and save a reference to the Animator Component
        anim = GetComponent<Animator>();

        // Check if Animator was added
        if (!anim)
            Debug.Log("Animator not found.");


        if (speed == 0)
        {
            speed = 0.70f;
        }

        if (health == 0)
        {
            health = 1;
        }

    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, 0);
        if (!isFacingLeft)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, 0);
        if (Input.GetKeyDown("g"))
        {
            
            rb.gravityScale *= -1;
            Vector3 scaleFactor = transform.localScale;

            // change sign of 'y' to cause a flip
            scaleFactor.y *= -1; // or - scaleFactor.x;

            // Assign updated scale to varible to cause flip
            transform.localScale = scaleFactor;
        }
        }

    void OnCollisionEnter2D(Collision2D c)
    {
        if(c.gameObject.tag == "EnemyBumper")
        {
            rb.velocity = Vector2.zero;
            flip();
        }

        if (c.gameObject.tag == "Projectile")
        {
            health--;
            if (health <= 0)
            {
                GetComponent<Rigidbody2D>().drag = 1000;
                SoundManager.instance.playSingleSound(death);
                rb.velocity = Vector2.zero;
                anim.SetTrigger("Death");
                PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + 200);
                Destroy(gameObject, 0.5f);
            }
            return;
        }

    } 

    void flip()
    {
        isFacingLeft = !isFacingLeft;

        // keep a copy of "localscale" to apply scale flip
        Vector3 scaleFactor = transform.localScale;

        // change sign of 'x' to cause a flip
        scaleFactor.x *= -1; // or - scaleFactor.x;

        // Assign updated scale to varible to cause flip
        transform.localScale = scaleFactor;

    }
}
