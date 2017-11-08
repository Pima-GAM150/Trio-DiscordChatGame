using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float hp = 2f;
    public float dmg = 1f;
    public float currentIncome = 0f;

	// Use this for initialization
	void Start () {
        currentIncome = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(hp <= 0)
        {
            Destroy(this.gameObject);
        }
	}

}
