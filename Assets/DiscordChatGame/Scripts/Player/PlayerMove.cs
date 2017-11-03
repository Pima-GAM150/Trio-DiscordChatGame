using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    public Rigidbody2D rb;
    public float speed = 1f;

    private float moveX;
    private float moveY;

    // Use this for initialization
    void Start () {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        Physics2D.gravity = Vector2.zero;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //create a vector3 with where the mouse position is located in the camera
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //rotate player based on where they are looking and where the mouse is
        transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);

        //anim.SetTrigger("playerWalking");
        moveX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveX * speed, rb.velocity.y);

        moveY = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(rb.velocity.x, moveY * speed);
    }
}
