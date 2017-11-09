using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour {

    public State currentState;
    public State remainState;
    public float stateTimeElapsed;
    public GameObject player;

    public float speed = 1f;
    public float maxForce = 2f;
    public Rigidbody2D rb;
    public float sightRange = 2f;
    public float width;
    public float distanceFromWall = 4.07f;
    public bool faceRight = true;

    public Animator animator;
    public bool hitPlayer;

    private bool isActive = true;
    

	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody2D>();
        width = this.GetComponent<SpriteRenderer>().bounds.extents.x;
        player = GameObject.Find("Player");
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
        if(hitPlayer)
        {
            //Debug.Log("Setting hitPlayer back to false.");
            //player.GetComponent<PlayerController>().hp -= this.gameObject.GetComponent<Enemy>().dmg;
            hitPlayer = false;
        }
	}

    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
            OnExitState();
        }
    }


    public void Flip(bool faceRight)
    {
        if (faceRight)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    
    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState()
    {
        stateTimeElapsed = 0;
    }
    
}
