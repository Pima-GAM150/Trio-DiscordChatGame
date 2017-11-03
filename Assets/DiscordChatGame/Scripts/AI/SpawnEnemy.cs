using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour {

    public GameObject player;
    public List<GameObject> prefabs;
    public static List<EnemyDictionary> enemyDictionary;
    public List<GameObject> enemyList;
    
    // Use this for initialization
    void Start () {
        instantiateEnemy("EnemyCircle", "Enemy", 5);
    }
	
	// Update is called once per frame
	void Update () {
        incrementEnemyTimeAlive();
        foreach(GameObject enemy in enemyList)
        {
            Debug.Log(enemy.GetComponent<Enemy>().timeAlive);
        }
	}

    public void instantiateEnemy(string enemyName, string userName, int spawnNumber)
    {
        GameObject prefab = getValue(enemyName);

        for(int i = 0; i < spawnNumber; i++)
        {
            Vector2 position = getRandomPosition();
            GameObject newEnemy = (GameObject)Instantiate(prefab, position, Quaternion.identity);
            newEnemy.name = userName;
            enemyList.Add(newEnemy);
        }

    }


    public void instantiateEnemy(GameObject prefab, Vector2 position)
    {
        GameObject newEnemy = (GameObject)Instantiate(prefab, position, Quaternion.identity);
    }

    public Vector2 getRandomPosition()
    {
        Transform location = player.transform;
        float ang = Random.value * 360;
        float radius = 10f;
        Vector2 pos;
        pos.x = location.position.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = location.position.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }

    public void incrementEnemyTimeAlive()
    {
        checkForNulls(enemyList);
        foreach(GameObject enemy in enemyList)
        {
            enemy.GetComponent<Enemy>().timeAlive += Time.deltaTime;
        }   
    }

    public void checkForNulls(List<GameObject> enemyList)
    {
        foreach (GameObject obj in enemyList)
        {
            if (obj == null)
            {
                enemyList.Remove(obj);
            }
        }

    }

    public GameObject getValue(string key)
    {
        Debug.Log(enemyDictionary.Count);
        foreach(EnemyDictionary enemy in enemyDictionary)
        {
            Debug.Log(enemy.key);
            if(enemy.key.Contains(key))
            {
                Debug.Log("Found a match!");
                return enemy.value;
            }
        }
        return null;
    }

}

[System.Serializable]
public class EnemyDictionary
{
    public string key;
    public GameObject value;

}
