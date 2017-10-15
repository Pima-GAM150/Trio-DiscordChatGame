using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/State")]
public class State : ScriptableObject {

    public Action[] actions;
    public Transitions[] transitions;

    //Tries to do whatever the action is, then checks to see what the transition to the next state will be.
    public void UpdateState(StateController controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    //This will do a 'i' number of actions that in the actions array
    private void DoActions(StateController controller)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(controller);
        }
    }

    //Check to see if the decision criteria is met, if it is then it will transition to that state, otherwise it will check the next state
    private void CheckTransitions(StateController controller)
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            bool decisionSucceeded = transitions[i].decision.Decide(controller);
            //Debug.Log(decisionSucceeded);

            if (decisionSucceeded)
            {
                //Debug.Log(transitions[i].trueState);
                controller.TransitionToState(transitions[i].trueState);
            }
            else
            {
                controller.TransitionToState(transitions[i].falseState);
            }

        }
    }
}
