using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public Rigidbody2D rb;
    public GameObject player;

	// Use this for initialization
	void Start () {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        lookWhereMoving();
	}

    public void lookWhereMoving()
    {
        //use the velocity as the direction you want to look at so that the vehicle will face the direction the force is being applied towards
        Vector2 direction = rb.velocity;
        Quaternion newRotation = Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime);

        //Quaternion newRotation = Quaternion.LookRotation(player.transform.position - transform.position, Vector2.right);
        //newRotation.x = 0f;
        //newRotation.y = 0f;

        //transform.rotation = newRotation;
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 2f * Time.deltaTime);
    }
}
