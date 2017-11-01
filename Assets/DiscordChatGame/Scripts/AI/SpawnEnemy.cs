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
            Transform location = player.transform;
            float ang = Random.value * 360;
            float radius = 10f;
            Vector2 pos;
            pos.x = location.position.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
            pos.y = location.position.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
            instantiateEnemy(prefabs[0], pos);
        }
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void instantiateEnemy(GameObject prefab, Vector2 position)
    {
        GameObject newEnemy = (GameObject)Instantiate(prefab, position, Quaternion.identity);
    }

    /*
     *
     *          Transform location = player.transform;
     *          float ang = Random.value * 360;
                float radius = 10f;
                Vector2 pos;
                pos.x = location.position.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
                pos.y = location.position.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad); 
     */
}
