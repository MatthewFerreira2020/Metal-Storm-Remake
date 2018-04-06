using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class turret : MonoBehaviour {
    public float fireRate;
    public Transform ProjectileSpawnPoint;
    public Transform ProjectileSpawnPointRight;
    public EnemyProjectile enemyprojectile;
    public float projectileForce;
    public bool isFacingLeft;
    Animator anim;
    public int health = 5;

    public character target;
    public bool findtarget;
    float timeSinceLastFire = 0;

    public AudioClip death;
    public AudioClip shoot;
    public AudioClip damage;

    // Use this for initialization
    void Start()
    {
        // Used to get and save a reference to the Animator Component
        anim = GetComponent<Animator>();

        // Check if Animator was added
        if (!anim)
            Debug.Log("Animator not found.");

        if (projectileForce == 0)
        {
            projectileForce = 2.5f;
        }

        if (fireRate == 0)
        {
            fireRate = 2.0f;
        }

        if (!enemyprojectile)
        {
            Debug.Log("projectile is not set");
        }

        if (!ProjectileSpawnPoint)
        {
            Debug.Log("projectilespawnpoint is not set");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "level 1" && findtarget == false)
        {
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<character>();
            findtarget = true;
        }
        if(SceneManager.GetActiveScene().name == "Title")
        {
            findtarget = false;
        }

        if (target.transform.position.x > transform.position.x)
        {
            anim.SetBool("changedirection", true);
        }
        else
        {
            anim.SetBool("changedirection", false);
        }
    }

    void OnTriggerStay2D(Collider2D c)
    {
        // check if GameObject tagged as 'Player' is in the tigger area
        if (c.gameObject.tag == "Player")
        {
            if (Time.time > timeSinceLastFire + fireRate)
            {
                
                fire();
                timeSinceLastFire = Time.time;
            }

        }
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Projectile")
        {
            SoundManager.instance.playSingleSound(damage);
            health--;
            if (health <= 0 || target.gunPowerUp == true)
            {
                SoundManager.instance.playSingleSound(death);
                anim.SetTrigger("death");
                PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + 300);
                Destroy(gameObject, 0.5f);
            }
            return;
        }
    }
    void fire()
    {
        shootDirectionCheck();
        SoundManager.instance.playSingleSound(shoot);
        if (isFacingLeft == true)
        {
            EnemyProjectile p = Instantiate(enemyprojectile, ProjectileSpawnPoint.position, ProjectileSpawnPoint.rotation) as EnemyProjectile;
            p.GetComponent<Rigidbody2D>().AddForce(Vector2.left * projectileForce, ForceMode2D.Impulse);
            Vector3 scaleFactor = p.transform.localScale;
            scaleFactor.x *= -1;
            p.transform.localScale = scaleFactor;
        }
        else
        {
            EnemyProjectile p = Instantiate(enemyprojectile, ProjectileSpawnPointRight.position, ProjectileSpawnPointRight.rotation) as EnemyProjectile;
            p.GetComponent<Rigidbody2D>().AddForce(Vector2.right * projectileForce, ForceMode2D.Impulse);
        }
    }

    void shootDirectionCheck()
    {
        if (target.transform.position.x < transform.position.x)
        {
            isFacingLeft = true;
            
        }
        else
        {
            isFacingLeft = false;
            
        }
    }

    

}
