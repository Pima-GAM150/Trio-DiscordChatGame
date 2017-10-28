using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {

    public Rigidbody2D player;



	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        transform.position = new Vector3(player.position.x, player.position.y, -10f);
	}
}
