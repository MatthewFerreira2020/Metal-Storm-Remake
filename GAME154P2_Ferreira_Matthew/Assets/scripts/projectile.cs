using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour {
    float delay = 0.25f;
    public float timer;
    public Animator anim;
    // Use this for initialization
    void Start() {
        Physics2D.IgnoreLayerCollision(11, 11, true);
        Physics2D.IgnoreLayerCollision(11, 10, true);
        Physics2D.IgnoreLayerCollision(11, 9, true);
        Physics2D.IgnoreLayerCollision(11, 12, true);
        Physics2D.IgnoreLayerCollision(11, 16, true);

        anim = GetComponent<Animator>();

        // Check if Animator was added
        if (!anim)
            Debug.Log("Animator not found.");
}
	
	// Update is called once per frame
	void Update () {
        timer += 1.0F * Time.deltaTime;
        if (timer >= 1)
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D c)
    {
            GetComponent<Rigidbody2D>().drag = 1000;
            GetComponent<CircleCollider2D>().enabled = false;
            anim.SetBool("Hit", true);
            Destroy(gameObject, 0.2f);
    }
}

