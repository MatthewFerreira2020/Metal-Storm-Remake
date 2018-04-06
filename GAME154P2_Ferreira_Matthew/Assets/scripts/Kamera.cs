using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamera : MonoBehaviour {
    public GameObject target;

    public Vector2 targetPosition;

    public Vector2 velocity;
    public float smoothTime;
	// Use this for initialization
	void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {
        targetPosition = new Vector2(Mathf.SmoothDamp(transform.position.x, target.transform.position.x, ref velocity.x, smoothTime), transform.position.y);

        transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
	}
}
