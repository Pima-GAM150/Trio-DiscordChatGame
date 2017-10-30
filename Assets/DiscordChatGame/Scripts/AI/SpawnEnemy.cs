using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour {

    public GameObject player;
    public List<GameObject> prefabs;

	// Use this for initialization
	void Start () {
        instantiateEnemy(prefabs[0], new Vector2(-1, 3));
        for (int i = 0; i < 5; i++)
        {
            instantiateEnemy(prefabs[0], new Vector2(i - 1, 3));
        }
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void instantiateEnemy(GameObject prefab, Vector2 position)
    {
        GameObject newEnemy = (GameObject)Instantiate(prefab, position, Quaternion.identity);
    }
}
