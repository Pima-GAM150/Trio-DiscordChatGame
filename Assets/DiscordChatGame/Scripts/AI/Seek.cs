using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour {

    public Transform player;
    public Rigidbody2D rb;

    private float maxForce = 1f;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        //ApplyForce(seek(player.position, 1f));
	}

    public Vector3 seek(Vector3 target, float weight)
    {

        Vector3 desired = target - transform.position;
        return desired *= weight;

    }

    public void ApplyForce(Vector2 force)
    {
        rb.velocity += force * Time.deltaTime;
        //Debug.Log(rb.velocity);
        if (rb.velocity.magnitude > maxForce)
        {
            rb.velocity = rb.velocity.normalized * maxForce;
        }
    }
}
