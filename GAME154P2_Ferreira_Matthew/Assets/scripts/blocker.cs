using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blocker : MonoBehaviour {
    protected float speed;
    protected int health;
    Animator anim;
    Rigidbody2D rb;
    public AudioClip death;
    public AudioClip damage;

    protected bool playerinrange = false;
    // Use this for initialization
    void Start () {
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
            speed = 1f;
        }

        if (health == 0)
        {
            health = 20;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (playerinrange == true)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
        
    }

     void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Projectile")
        {
            SoundManager.instance.playSingleSound(damage);
            health--;
            if (health <= 0)
            {
                rb.velocity = Vector2.zero;
                SoundManager.instance.playSingleSound(death);
                anim.SetTrigger("death");
                PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + 5000);
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
