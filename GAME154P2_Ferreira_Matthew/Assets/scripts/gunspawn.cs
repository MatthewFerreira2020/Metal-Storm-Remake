using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunspawn : MonoBehaviour {
    public Transform GunSpawnPoint;
    public Powerup powerup;
    float timer;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 30)
        {
            Powerup p = Instantiate(powerup, new Vector3(GunSpawnPoint.position.x, GunSpawnPoint.position.y, GunSpawnPoint.position.z), GunSpawnPoint.rotation) as Powerup;

            timer = 0;
            Destroy(p, 31);
        }
    }
}
