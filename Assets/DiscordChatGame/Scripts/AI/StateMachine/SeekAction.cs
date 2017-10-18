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
        Vector2 playerDirection = controller.transform.position - controller.player.transform.position;
        playerDirection.Normalize();
        Move(controller, playerDirection);
        

    }

    private void Move(StateController controller, Vector2 playerDirection)
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

    private void FacePlayer(StateController controller)
    {
        controller.transform.rotation = Quaternion.LookRotation(Vector2.right, controller.player.transform.position - controller.transform.position);
    }

}
