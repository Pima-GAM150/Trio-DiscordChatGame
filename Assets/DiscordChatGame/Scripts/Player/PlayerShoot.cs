using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

    public GameObject bulletPrefab;
    public GameObject spawnPosition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
        {
            instantiateBullet(bulletPrefab, spawnPosition.transform.position);
        }
	}

    public void instantiateBullet(GameObject bullet, Vector2 position)
    {
        GameObject newBullet = (GameObject)Instantiate(bullet, position, Quaternion.identity);
    }
}
