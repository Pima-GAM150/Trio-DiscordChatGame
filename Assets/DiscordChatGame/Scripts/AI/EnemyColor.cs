using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyColor : MonoBehaviour {

    public SpriteRenderer sprite;


	// Use this for initialization
	void Start () {

        Color color = sprite.color;
        color.r = Random.Range(0f, 1f);
        color.b = Random.Range(0f, 1f);
        color.g = Random.Range(0f, 1f);

        sprite.color = color;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
