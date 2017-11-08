using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        incomeMultiplier = cost / 4f;
        //animator.Play("EnemySwordStab");
    }
	
	// Update is called once per frame
	void Update () {
        lookWhereMoving();
        ApplyForce(seperation.Separate(nearByObjects));
        if(hp <= 0)
        {
            Debug.Log("Dead!");
            onDeath();
        }
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

    public void onDeath()
    {
        //Wrapping this in an empty check otherwise it never makes it to the Destroy function
        if(SpawnEnemy.enemyIncome.Count > 0)
        {
            EnemyIncome userIncome;
            userIncome = SpawnEnemy.enemyIncome.First<EnemyIncome>(s => s.key.Equals(name));
            userIncome.value += timeAlive * incomeMultiplier;
            Debug.Log(userIncome.value);
            
        }
        player.GetComponent<PlayerController>().currentIncome += cost;
        Destroy(this.gameObject);
    }
}
