using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/ToSeek")]
public class ToSeek : Decision {

    public override bool Decide(StateController controller)
    {
        //Debug.Log(checkForTarget(controller));
        return checkForTarget(controller);
    }

    private bool checkForTarget(StateController controller)
    {
        Debug.Log(Vector2.Distance(controller.player.transform.position, controller.transform.position));
        return Vector2.Distance(controller.player.transform.position, controller.transform.position) < controller.sightRange;
    }

    private bool checkForTarget(StateController controller, String name)
    {
        RaycastHit2D hit;
        Vector2 lineCastPosition;
        //RaycastHit2D[] hits;

        if (controller.faceRight)
        {
            lineCastPosition = controller.transform.position + controller.transform.right * controller.width;
            //hits = Physics2D.LinecastAll(lineCastPosition, controller.player.transform.position);
            hit = Physics2D.Raycast(lineCastPosition, Vector2.right);
            Debug.DrawRay(lineCastPosition, Vector2.right, Color.red);
        }
        else
        {
            lineCastPosition = controller.transform.position + controller.transform.right * controller.width;
            //hits = Physics2D.LinecastAll(lineCastPosition, controller.player.transform.position);
            hit = Physics2D.Raycast(lineCastPosition, -Vector2.right);
            Debug.DrawRay(lineCastPosition, -Vector2.right, Color.red);
        }

        //foreach (RaycastHit2D hit in hits)
        //{
        //Debug.Log(hit.collider.name);
        if (hit.collider.name == name && Vector2.Distance(controller.player.transform.position, controller.transform.position) < 5f)
        {
            Debug.Log("Player Detected!");
            return true;
        }
        else
        {
            return false;
        }
        //}
        //return false;

    }

}
