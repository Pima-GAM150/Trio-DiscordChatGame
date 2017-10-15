using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour {

    public State currentState;
    public State remainState;
    public float stateTimeElapsed;

    public float speed = 1f;
    public Rigidbody2D rb;
    public float width;
    public float distanceFromWall = 4.07f;
    public bool faceRight = true;

    private bool isActive = true;

	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody2D>();
        width = this.GetComponent<SpriteRenderer>().bounds.extents.x;
    }
	
	// Update is called once per frame
	void Update () {
		if(!isActive)
        {
            return;
        } else
        {
            currentState.UpdateState(this);
        }
	}

    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
            //OnExitState();
        }
    }

    /*
public bool CheckIfCountDownElapsed(float duration)
{
    stateTimeElapsed += Time.deltaTime;
    return (stateTimeElapsed >= duration);
}

private void OnExitState()
{
    stateTimeElapsed = 0;
}
*/
}
