using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/ToPatrol")]
public class ToPatrol : Decision {

    public override bool Decide(StateController controller)
    {
        return checkForTarget(controller);
    }

    //Currently not implemented, will need to check to see if the player is visiable or not
    private bool checkForTarget(StateController controller)
    {
        return false;
    }

}
