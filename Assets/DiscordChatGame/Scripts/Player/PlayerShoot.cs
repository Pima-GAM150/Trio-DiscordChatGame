using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

    public GameObject bulletPrefab;
    public GameObject spawnPosition;
    public float shootDelay = 0.25f;

    private float shootTimer = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        shootTimer += Time.deltaTime;
		if(Input.GetMouseButtonDown(0))
        {
            if(shootTimer > shootDelay)
            {
                instantiateBullet(bulletPrefab, spawnPosition.transform.position);
                shootTimer = 0f;
            }
            
        }
	}

    public void instantiateBullet(GameObject bullet, Vector2 position)
    {
        GameObject newBullet = (GameObject)Instantiate(bullet, position, Quaternion.identity);
    }
}
