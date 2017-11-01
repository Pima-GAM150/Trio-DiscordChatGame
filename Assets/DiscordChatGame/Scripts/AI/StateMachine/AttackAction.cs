using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Actions/AttackAction")]
public class AttackAction : Action {

    bool playerHit = false;

    public override void Act(StateController controller)
    {
        BasicAttack(controller);
    }

    public void BasicAttack(StateController controller)
    {
        //Debug.Log("Playing the Animation!");
        //Debug.Log(playerHit);
        controller.animator.Play("EnemySwordStabAnimation");
    }




}
