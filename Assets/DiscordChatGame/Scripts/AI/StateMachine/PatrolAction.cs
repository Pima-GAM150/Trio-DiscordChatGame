using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Actions/PatrolAction")]
public class PatrolAction : Action {

    private Vector2 lineCastPosition;
    private bool grounded = true;
    private float jumpTimer = 0f;
    private float jump = 0.5f;

    public override void Act(StateController controller)
    {
        Patrol(controller);
    }

    private void Patrol(StateController controller)
    {
        jumpTimer += Time.deltaTime;
        //forward raycast
        RaycastHit2D hit;

        if (controller.faceRight)
        {
            lineCastPosition = controller.transform.position + controller.transform.right * controller.width;
            //lineCastPosition = controller.transform.position;
            hit = Physics2D.Raycast(lineCastPosition, Vector2.right);
            Debug.DrawRay(lineCastPosition, Vector2.right, Color.red);
        }
        else
        {
            lineCastPosition = controller.transform.position + controller.transform.right * controller.width;
            //lineCastPosition = controller.transform.position;
            hit = Physics2D.Raycast(lineCastPosition, -Vector2.right);
            Debug.DrawRay(lineCastPosition, -Vector2.right, Color.red);
        }
        
        //checks if enemy is grounded
        grounded = isGrounded(lineCastPosition);
        //Debug.Log(grounded);
        Debug.Log(hit.collider.name + ", " + Mathf.Abs(Vector2.Distance(controller.transform.position, hit.collider.transform.position)));
        float distanceFromHit = Mathf.Abs(Vector2.Distance(controller.transform.position, hit.collider.transform.position));
        if (hit.collider.tag == "Obstacle"
            && grounded
            && jumpTimer > jump
            && distanceFromHit < 1.5f)
        {
            jumpTimer = 0f;
            //Debug.Log("Raycast hit an obstacle");
            Jump(controller, 6f);
        }
        else if (hit.collider.name == "TallObstacle"
          && grounded
          && jumpTimer > jump
          && distanceFromHit < 1.5f)
        {
            jumpTimer = 0f;
            Jump(controller, 10f);
        }
        else if (hit.collider.name.Contains("RightWall")
            && distanceFromHit < controller.distanceFromWall)
        {
            controller.transform.localRotation = Quaternion.Euler(0, 180, 0);
            controller.faceRight = false;
        }
        else if (hit.collider.name.Contains("LeftWall")
          && distanceFromHit < controller.distanceFromWall)
        {
            controller.transform.localRotation = Quaternion.Euler(0, 0, 0);
            controller.faceRight = true;
        }

        Move(controller);
    }

    private void Move(StateController controller)
    {
        Vector2 velocity;
        if (controller.faceRight)
        {
            velocity = controller.rb.velocity;
            velocity.x = Vector2.right.x * controller.speed;
        }
        else
        {
            velocity = controller.rb.velocity;
            velocity.x = -Vector2.right.x * controller.speed;
        }

        controller.rb.velocity = velocity;
    }

    private void Jump(StateController controller, float jumpPower)
    {
        Debug.Log("Jumping!");
        controller.rb.velocity += new Vector2(10f, jumpPower);
        //rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    private bool isGrounded(Vector2 lineCastPosition)
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
