using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTriggerEnter : MonoBehaviour {

    public StateController controller;

	// Use this for initialization
	void Start () {
        controller = this.gameObject.GetComponentInParent<StateController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log(collider.name);
        if (collider.tag.Contains("Player"))
        {
            Debug.Log("Hit the player!");
            controller.hitPlayer = true;
            controller.player.GetComponent<PlayerController>().hp -= controller.gameObject.GetComponent<Enemy>().dmg;
        }
    }
}
