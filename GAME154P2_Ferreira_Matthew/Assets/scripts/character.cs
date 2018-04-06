using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class character : MonoBehaviour {
    // Makes a private reference to Rigidbody2D Component
    public Rigidbody2D rb;
    public AudioClip death;
    public AudioClip shoot;
    public AudioClip damage;
    public AudioClip jump;
    public AudioClip gravflip;
    // Makes a public reference to Rigidbody2D Component
    // - Shown in Inspector

    // Variable to control jumpForce of GameObject
    public float jumpForce = 2.0f;

    // Variable to control speed of GameObject
    public float speed;

    // variables to tell if character should jump or not
    public bool isGrounded;
    public LayerMask isGroundLayer;
    public Transform groundcheck;
    public float groundcheckradius;

    // gravity check
    public bool gravityFliped = false;
    public bool isFliping = false;

    // used for fliping character
    public bool isFacingLeft;

    //handles projectile spawning
    public Transform ProjectileSpawnPoint;
    public Transform TopProjectileSpawnPoint;
    public projectile projectile;
    public float projectileForce;

    //check if projectile fired up
    public bool upFire = false;

    // handles health
    public float health;

    //check if powerup is active
    public bool gunPowerUp = false;

    public float timer;

    public Animator anim;

   public Transform SpawnPoint;

    public int lives;


    // Use this for initialization
    void Start () {
        PlayerPrefs.SetInt("score", 0);

        Physics2D.IgnoreLayerCollision(12, 16, true);
        anim = GetComponent<Animator>();

        // Check if Animator was added
        if (!anim)
            Debug.Log("Animator not found.");
        // Used to get and save a reference to the Rigidbody2D Component
        rb = GetComponent<Rigidbody2D>();

        // Check if Rigidbody2D was added
        // - Not really needed because of [RequireComponent(typeof(Rigidbody2D))]
        // - That automagically adds a Rigidbody2D to GameObject
        if (!rb)
            Debug.Log("Rigidbody not found.");
        else
        {
            // Change variables of Rigidbody2D after saving a reference
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.mass = 1.0f;
        }
        // Check if speed variable was set in the inspector
        if (speed == 0)
        {
            // Assign a default value if one was not set
            speed = 1.25f;

            // Prints a warning message to the Console
            // - Open Console by going to Window-->Console (or Ctrl+Shift+C)
            Debug.LogWarning("Speed not set. Defaulting to " + speed);
        }
        // Check if groundcheckradius variable was set in the inspector
        if (groundcheckradius == 0)
        {
            groundcheckradius = 0.1f;
            // Prints a warning message to the Console
            // - Open Console by going to Window-->Console (or Ctrl+Shift+C)
            Debug.LogWarning("ground check radius not set. Defaulting to " + groundcheckradius);
        }
        // Check if groundcheck variable was set in the inspector
        if (!groundcheck)
        {
            // Prints a warning message to the Console
            // - Open Console by going to Window-->Console (or Ctrl+Shift+C)
            Debug.LogWarning("ground check not set in inspector.");
        }

        if (health == 0)
        {
            health = 1;
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (Time.timeScale == 1)
        {
            // set timer to seconds
            timer += 1.0F * Time.deltaTime;

            //toggle platform detection
            if (gravityFliped == false)
            {
                foreach (GameObject platform in GameObject.FindGameObjectsWithTag("Platform"))
                {
                    platform.GetComponent<EdgeCollider2D>().enabled = true;
                }
                foreach (GameObject invertedplatform in GameObject.FindGameObjectsWithTag("InvertedPlatform"))
                {
                    invertedplatform.GetComponent<EdgeCollider2D>().enabled = false;
                }
            }

            if (gravityFliped == true)
            {
                foreach (GameObject platform in GameObject.FindGameObjectsWithTag("Platform"))
                {
                    platform.GetComponent<EdgeCollider2D>().enabled = false;
                }
                foreach (GameObject invertedplatform in GameObject.FindGameObjectsWithTag("InvertedPlatform"))
                {
                    invertedplatform.GetComponent<EdgeCollider2D>().enabled = true;
                }
            }

            // Check if left or right keys are pressed
            // - Gives decimals from -1 to +1
            //float moveValue = Input.GetAxis("Horizontal");

            // - Gives -1, 0, +1
            float moveValue = Input.GetAxis("Horizontal");

            //check if character is touching anything labeled as Ground/Platform/Jumpable
            isGrounded = Physics2D.OverlapCircle(groundcheck.position, groundcheckradius, isGroundLayer);

            //check if character is grounded
            if (isGrounded)
            {
                // Check if Jump was pressed (aka Space)
                if (Input.GetButton("Jump"))
                {
                    if (jumpForce > 3.5f)
                    {
                        return;
                    }
                    jumpForce += 0.5f;
                    //rb.AddForce(new Vector2(0, 10.0f), ForceMode2D.Impulse);

                    // Vector2.up --> new Vector2(0,1)
                    // Vector2.down --> new Vector2(0,-1)
                    // Vector2.left --> new Vector2(-1,0)
                    // Vector2.right --> new Vector2(1,0)
                    // Vector2.zero --> new Vector2(0,0)
                    // Vector2.one --> new Vector2(1,1)


                }
                if (gravityFliped == false)
                {
                    if (Input.GetButtonUp("Jump"))
                    {
                        // Applies a force in UP direction
                        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                        SoundManager.instance.playSingleSound(jump);
                        jumpForce = 1.5f;
                        anim.SetBool("Jumping", true);
                        StartCoroutine(StopAnimation());
                    }
                }
                if (gravityFliped == true)
                {
                    if (Input.GetButtonUp("Jump"))
                    {
                        // Applies a force in UP direction
                        rb.AddForce(Vector2.down * jumpForce, ForceMode2D.Impulse);
                        SoundManager.instance.playSingleSound(jump);
                        jumpForce = 1.5f;
                        anim.SetBool("Jumping", true);
                        StartCoroutine(StopAnimation());
                    }
                }

            }
            if (gravityFliped == false)
            {
                if (Input.GetKey("w") && Input.GetButtonDown("Fire1"))
                {
                    SoundManager.instance.playSingleSound(shoot);
                    upFire = true;
                    projectileForce = 2.5f;
                    projectile temp = Instantiate(projectile, TopProjectileSpawnPoint.position, TopProjectileSpawnPoint.rotation) as projectile;
                    temp.GetComponent<Rigidbody2D>().AddForce(Vector2.up * projectileForce, ForceMode2D.Impulse);
                    if (gunPowerUp == true)
                    {
                        temp.anim.SetBool("Strong3", true);
                    }
                    anim.SetBool("Shooting", true);
                    StartCoroutine(StopAnimation());
                }
            }
            if (gravityFliped == true)
            {
                if (Input.GetKey("s") && Input.GetButtonDown("Fire1"))
                {
                    SoundManager.instance.playSingleSound(shoot);
                    upFire = true;
                    projectileForce = 2.5f;
                    projectile temp = Instantiate(projectile, TopProjectileSpawnPoint.position, TopProjectileSpawnPoint.rotation) as projectile;
                    temp.GetComponent<Rigidbody2D>().AddForce(Vector2.down * projectileForce, ForceMode2D.Impulse);
                    if (gunPowerUp == true)
                    {
                        temp.anim.SetBool("Strong4", true);
                    }
                    anim.SetBool("Shooting", true);
                    StartCoroutine(StopAnimation());
                }
            }

            if (Input.GetKeyDown("g") && isFliping == false)
            {
                flipGravity();
                isFliping = true;
                anim.SetBool("GravitySwap", true);
            }

            // Make player move left or right based off moveValue
            rb.velocity = new Vector2(moveValue * speed, rb.velocity.y);
            //rb.AddForce(new Vector2(moveValue * speed, rb.velocity.y));
            if (moveValue != 0)
            {
                anim.SetBool("Moving", true);

            }
            if (moveValue == 0)
            {
                anim.SetBool("Moving", false);
            }

            if (moveValue < 0 && !isFacingLeft)
            {
                flip();
            }
            else if (moveValue > 0 && isFacingLeft)
            {
                flip();
            }
            if (upFire == false)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    SoundManager.instance.playSingleSound(shoot);
                    fireProjectile();
                }
            }

            upFire = false;

        }
    }
    public void flipGravity()
    {
        SoundManager.instance.playSingleSound(gravflip);
        rb.gravityScale *= -1;
        if (rb.gravityScale == 1)
        {
            gravityFliped = false;
        }
        if(rb.gravityScale == -1)
        {
            gravityFliped = true;
        }
        Vector3 scaleFactor = transform.localScale;

        // change sign of 'x' to cause a flip
        scaleFactor.y *= -1; // or - scaleFactor.x;

        // Assign updated scale to varible to cause flip
        transform.localScale = scaleFactor;
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
    void fireProjectile()
    {
        
        Debug.Log("pew");
        projectileForce = 2.5f;
        projectile temp = Instantiate(projectile, ProjectileSpawnPoint.position, ProjectileSpawnPoint.rotation) as projectile;
        if (isFacingLeft == false)
        {
            temp.GetComponent<Rigidbody2D>().AddForce(Vector2.right * projectileForce, ForceMode2D.Impulse);
            if(gunPowerUp == true)
            {
                temp.anim.SetBool("Strong", true);
            }
        }
        else
        {
            temp.GetComponent<Rigidbody2D>().AddForce(Vector2.left * projectileForce, ForceMode2D.Impulse);
            if (gunPowerUp == true)
            {
                temp.anim.SetBool("Strong2", true);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        
        if (c.gameObject.tag == "Ground" || c.gameObject.tag == "Platform" || c.gameObject.tag == "InvertedPlatform")
        {
            isFliping = false;
            anim.SetBool("GravitySwap", false);
        }
        if (timer >= 3.0f)
        {
            if (c.gameObject.tag == "Enemy" || c.gameObject.tag == "EnemyProjectile")
            {
                SoundManager.instance.playSingleSound(damage);
                health -= 1;
                Debug.Log("player health = " + health);
                if (health == 0)
                {
                    SoundManager.instance.playSingleSound(death);
                    anim.SetBool("Dead", true);
                    lives--;
                    gunPowerUp = false;
                    GameManager.Instance.ChangeLevelCanvas();
                }
                else
                {
                    anim.SetBool("Damaged", true);
                    StartCoroutine(StopAnimation());
                }
                timer = 0.0f;
            }
        }
        if(c.gameObject.tag == "GunPowerup")
        {
            gunPowerUp = true;
            anim.SetBool("Powering", true);
            StartCoroutine(StopAnimation());
        }
        if(c.gameObject.tag == "ArmorPowerup")
        {
            if (health == 1)
            {
                health += 2;
            }
            if(health == 2)
            {
                health += 1;
            }
            Debug.Log(health);
            anim.SetBool("Powering", true);
            StartCoroutine(StopAnimation());
        }
    }

    public IEnumerator StopAnimation()
    {
        if(anim.GetBool("Damaged") == true)
        {
            yield return new WaitForSeconds(2f);
            anim.SetBool("Damaged", false);
        }

        if (anim.GetBool("Shooting") == true)
        {
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("Shooting", false);
        }
        if (anim.GetBool("Jumping") == true)
        {
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("Jumping", false);
        }

        if (anim.GetBool("Powering") == true)
        {
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("Powering", false);
        }

    }


}
