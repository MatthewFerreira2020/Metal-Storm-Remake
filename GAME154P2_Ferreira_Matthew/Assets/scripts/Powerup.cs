using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {
    Animator anim;
    public AudioClip death;
    // Use this for initialization
    void Start () {
        Physics2D.IgnoreLayerCollision(15, 11, true);
        Physics2D.IgnoreLayerCollision(15, 13, true);
        Physics2D.IgnoreLayerCollision(15, 14, true);
        Physics2D.IgnoreLayerCollision(15, 15, true);
    }
	
	// Update is called once per frame
	void Update () {
        // Used to get and save a reference to the Animator Component
        anim = GetComponent<Animator>();

        // Check if Animator was added
        if (!anim)
            Debug.Log("Animator not found.");
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        anim.SetBool("Destroyed", true);
        SoundManager.instance.playSingleSound(death);
        PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + 1000);
        Destroy(gameObject, 0.5f);
    }
}
