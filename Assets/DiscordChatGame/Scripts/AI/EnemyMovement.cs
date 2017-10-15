using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    public LayerMask enemyMask;
    public float speed = 1f;
    public Rigidbody2D rb;
    public float width;
    public Vector2 lineCastPosition;
    public float distanceFromWall = 4.07f;

    private bool grounded = true;
    private float jumpTimer = 0f;
    private float jump = 0.5f;
    private bool faceRight = true;

    // Use this for initialization
    void Start () {
        rb = this.GetComponent<Rigidbody2D>();
        width = this.GetComponent<SpriteRenderer>().bounds.extents.x;
        lineCastPosition = this.transform.position + transform.right * width;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        jumpTimer += Time.deltaTime;
        //forward raycast
        RaycastHit2D hit;
        lineCastPosition = this.transform.position + transform.right * width;
        if (faceRight)
        {
            hit = Physics2D.Raycast(lineCastPosition, lineCastPosition + Vector2.right * 2f);
            Debug.DrawLine(lineCastPosition, lineCastPosition + Vector2.right * 2f, Color.red);
        } else
        {
            hit = Physics2D.Raycast(lineCastPosition, lineCastPosition + -Vector2.right * 2f);
            Debug.DrawLine(lineCastPosition, lineCastPosition + -Vector2.right * 2f, Color.red);
        }
        
        
        

        //checks if enemy is grounded
        grounded = isGrounded(lineCastPosition);
        //Debug.Log(grounded);
        Debug.Log(hit.collider.name + ", " + Mathf.Abs(Vector2.Distance(this.transform.position, hit.collider.transform.position)));
        if(hit.collider.tag == "Obstacle" 
            && grounded 
            && jumpTimer > jump)
        {
            jumpTimer = 0f;
            //Debug.Log("Raycast hit an obstacle");
            Jump(6f);
        } else if(hit.collider.name == "TallObstacle" 
            && grounded 
            && jumpTimer > jump)
        {
            jumpTimer = 0f;
            Jump(10f);
        }
        else if(hit.collider.name.Contains("RightWall")
            && Mathf.Abs(Vector2.Distance(this.transform.position, hit.collider.transform.position)) < distanceFromWall)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            faceRight = false;
        } else if(hit.collider.name.Contains("LeftWall")
            && Mathf.Abs(Vector2.Distance(this.transform.position, hit.collider.transform.position)) < distanceFromWall)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            faceRight = true;
        }

        Move();


	}
    /*
    void OnCollisionEnter2D(Collision2D collider)
    {
        Debug.Log(collider.gameObject.name);
        if(collider.gameObject.tag == "Obstacle")
        {
            if(grounded)
            {
                Jump();
            }
            
        }
    }
    */
    public void Move()
    {
        Vector2 velocity;
        if (faceRight)
        {
            velocity = rb.velocity;
            velocity.x = Vector2.right.x * speed;
        } else
        {
            velocity = rb.velocity;
            velocity.x = -Vector2.right.x * speed;
        }

        rb.velocity = velocity;
    }

    public void Jump(float jumpPower)
    {
        Debug.Log("Jumping!");
        rb.velocity += new Vector2(10f, jumpPower);
        //rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    public bool isGrounded(Vector2 lineCastPosition)
    {
        Debug.DrawLine(lineCastPosition, lineCastPosition + Vector2.down, Color.blue);
        //Debug.Log(Physics2D.Linecast(lineCastPosition, lineCastPosition + Vector2.down).collider.name);
        RaycastHit2D hit = Physics2D.Linecast(lineCastPosition, lineCastPosition + Vector2.down);

        if (hit.collider != null && hit.collider.name == "Floor")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
