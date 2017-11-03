using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Enemy {

    public Rigidbody2D rb;
    public GameObject player;
    public Animator animator;
    public List<GameObject> nearByObjects;
    public Separation seperation;

    public float maxForce = 5f;

    // Use this for initialization
    void Start () {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        //animator.Play("EnemySwordStab");
    }
	
	// Update is called once per frame
	void Update () {
        lookWhereMoving();
        ApplyForce(seperation.Separate(nearByObjects));
	}

    public void lookWhereMoving()
    {
        //use the velocity as the direction you want to look at so that the vehicle will face the direction the force is being applied towards
        Vector2 direction = rb.velocity;
        Quaternion newRotation = Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 5f);
    }

    private void ApplyForce(Vector2 force)
    {

        rb.velocity += force * Time.deltaTime;
        //Debug.Log(controller.rb.velocity);
        if (rb.velocity.magnitude > maxForce)
        {
            rb.velocity = rb.velocity.normalized * maxForce;
        }

    }

    //Adds all near by objects to a list/hashset
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //get a list of targets
        //send those to a separation function
        if (!collider.name.Equals("Player") && collider.gameObject != this.gameObject)
        {
            nearByObjects.Add(collider.gameObject);
        }


    }

    //Removes from list/hashset once they leave
    private void OnTriggerExit2D(Collider2D collider)
    {
        nearByObjects.Remove(collider.gameObject);
    }
}
