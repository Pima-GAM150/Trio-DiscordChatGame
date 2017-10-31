using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/ToAttack")]
public class ToAttack : Decision {

    public override bool Decide(StateController controller)
    {
        return InRange(controller);
    }

    private bool InRange(StateController controller)
    {
        return Vector2.Distance(controller.player.transform.position, controller.transform.position) < controller.sightRange;
    }

}
