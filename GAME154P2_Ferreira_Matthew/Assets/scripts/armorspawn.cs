using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class armorspawn : MonoBehaviour {
    public Transform ArmorSpawnPoint;
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
            Powerup p = Instantiate(powerup, new Vector3(ArmorSpawnPoint.position.x, ArmorSpawnPoint.position.y, ArmorSpawnPoint.position.z), ArmorSpawnPoint.rotation) as Powerup;

            timer = 0;
            Destroy(p, 31);
        }
    }
}
