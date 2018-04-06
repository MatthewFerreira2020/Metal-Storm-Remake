using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectiblespawn : MonoBehaviour {
    public Transform CollectibleSpawnPoint;
    public Collectible collectible;
    float timer;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(timer >= 30)
        {
            Collectible c = Instantiate(collectible, new Vector3(CollectibleSpawnPoint.position.x, CollectibleSpawnPoint.position.y, CollectibleSpawnPoint.position.z), CollectibleSpawnPoint.rotation) as Collectible;

            timer = 0;
            Destroy(c, 31);
        }
    }
}
