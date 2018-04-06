using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {
    float delay = 1.5f;
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {


    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if(c.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
        if (c.gameObject.tag != "Projectile" || c.gameObject.tag != "EnemyProjectile")
        {
            StartCoroutine(Destroy());
        }
    }
    public IEnumerator Destroy()
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
