using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {
    Animator anim;
    public AudioClip death;
    // Use this for initialization
    void Start () {
        Physics2D.IgnoreLayerCollision(14, 11, true);
        Physics2D.IgnoreLayerCollision(14, 13, true);
        Physics2D.IgnoreLayerCollision(14, 14, true);
        Physics2D.IgnoreLayerCollision(14, 15, true);

        // Used to get and save a reference to the Animator Component
        anim = GetComponent<Animator>();

        // Check if Animator was added
        if (!anim)
            Debug.Log("Animator not found.");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D c)
    {
        anim.SetBool("Destroyed", true);
        SoundManager.instance.playSingleSound(death);
        PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + 5000);
        Destroy(gameObject, 0.5f);
    }

}
