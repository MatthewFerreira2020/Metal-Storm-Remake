using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss : MonoBehaviour {
    protected int health;
    public Transform[] ProjectileSpawnPoint;
    public BossProjectile bossprojectile;
    Animator anim;
    float projectileSpeed;
    public character character;

    public AudioClip death;
    public AudioClip shoot;
    public AudioClip damage;

    float timer;

    Vector3 shotdirection;
    
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();

        // Check if Animator was added
        if (!anim)
            Debug.Log("Animator not found.");
        if (health == 0)
        {
            health = 100;
        }
        if(projectileSpeed == 0)
        {
            projectileSpeed = 20f;
        }
        if (!bossprojectile)
        {
            Debug.Log("projectile is not set");
        }

    }

        // Update is called once per frame
        void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D c)
    {
        // check if GameObject tagged as 'Player' is in the tigger area
        if (c.gameObject.tag == "Player")
        {
            anim.SetTrigger("Active");
        }
    }
    void OnTriggerStay2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player")
        {
            timer += Time.deltaTime;
            if (timer > 3)
            {
                anim.SetBool("firing", true);
                SoundManager.instance.playSingleSound(shoot);
                fire();
                Debug.Log("fire");
                timer = 0;
            }

        }
    }
    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Projectile")
        {
            if (character.gunPowerUp == false)
            {
                SoundManager.instance.playSingleSound(damage);
                health--;
            }
            if(character.gunPowerUp == true)
            {
                SoundManager.instance.playSingleSound(damage);
                health -= 5;
            }
            Debug.Log("boss health " + health);
            if (health <= 0)
            {
                SoundManager.instance.playSingleSound(death);
                PlayerPrefs.SetInt("win", 1);
                PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + 1000 * character.lives);
                GameManager.Instance.LoadScoreScreen();
            }
            return;
        }
    }
    void fire()
    {
        for (int shotsfired = 0; shotsfired < 3; shotsfired++)
        {
            
            shotdirection = character.transform.position - transform.position;
            shotdirection.Normalize();
            BossProjectile p = Instantiate(bossprojectile, new Vector3(ProjectileSpawnPoint[shotsfired].position.x, ProjectileSpawnPoint[shotsfired].position.y, ProjectileSpawnPoint[shotsfired].position.z), ProjectileSpawnPoint[shotsfired].rotation) as BossProjectile;
            p.GetComponent<Rigidbody2D>().AddForce(shotdirection * projectileSpeed);
        }
                
    }

}
