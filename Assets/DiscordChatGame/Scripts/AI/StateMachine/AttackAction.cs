using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Actions/AttackAction")]
public class AttackAction : Action {

    public override void Act(StateController controller)
    {
        BasicAttack(controller);
    }

    public void BasicAttack(StateController controller)
    {
        controller.animator.Play("EnemySwordStabAnimation");
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag.Contains("Player"))
        {
            Debug.Log("Hit the player!");
        }
    }


}
