using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Actions/SeekAction")]
public class SeekAction : Action {

    public override void Act(StateController controller)
    {
        Seek(controller);
    }

    public void Seek(StateController controller)
    {
        Debug.Log("Seeking Player");
        Vector2 playerDirection = controller.player.transform.position - controller.transform.position;
        playerDirection.Normalize();
        Move(controller, playerDirection);
        

    }

    private void Move(StateController controller, Vector2 playerDirection)
    {
        Vector2 velocity;
        /*
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
        */
        ApplyForce(controller, playerDirection);
        //ApplyForce(controller, seek(controller, controller.player.transform.position, 1f));
    }

    private void FacePlayer(StateController controller)
    {
        controller.transform.rotation = Quaternion.LookRotation(Vector2.right, controller.player.transform.position - controller.transform.position);
    }

    public Vector2 seek(StateController controller, Vector2 target, float weight)
    {

        Vector3 desired = target - (Vector2)controller.transform.position;
        return desired *= weight;

    }

    public void ApplyForce(StateController controller, Vector2 force)
    {
        
        controller.rb.velocity += force * Time.deltaTime;
        //Debug.Log(rb.velocity);
        if (controller.rb.velocity.magnitude > controller.maxForce)
        {
            controller.rb.velocity = controller.rb.velocity.normalized * controller.maxForce;
        }
        
        //controller.transform.position += Vector3.right * 0.01f;
    }

}
