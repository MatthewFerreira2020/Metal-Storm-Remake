using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour {
    protected float speed;
    protected int health;
    Animator anim;
    protected bool playerinrange = false;
    Rigidbody2D rb;
    public AudioClip death;
    // Use this for initialization
    void Start () {
        Physics2D.IgnoreLayerCollision(18, 13, true);
        Physics2D.IgnoreLayerCollision(18, 16, true);
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
            speed = 1.75f;
        }

        if (health == 0)
        {
            health = 1;
        }

    }

    // Update is called once per frame
    void Update () {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, 0);
        if (playerinrange == true)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, 0);
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Projectile")
        {
            health--;
            if (health <= 0)
            {
                GetComponent<Rigidbody2D>().drag = 1000;
                SoundManager.instance.playSingleSound(death);
                rb.velocity = Vector2.zero;
                anim.SetTrigger("death");
                PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + 100);
                Destroy(gameObject, 0.5f);
            }
            return;
        }
    }
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player")
        {
            playerinrange = true;
        }
    }

}
