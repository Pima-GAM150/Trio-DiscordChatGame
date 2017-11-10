using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSharpPlus;
using DSharpPlus.Entities;
using System.Linq;

public class SpawnEnemy : MonoBehaviour {

    public static SpawnEnemy instance;

    public GameObject player;
    public GameObject gameArea;
    public List<GameObject> prefabs;
    public List<EnemyDictionary> enemyDictionary = new List<EnemyDictionary>(); //a list of all the enemy prefabs that can be spawned
    public List<EnemyIncome> enemyIncome = new List<EnemyIncome>(); //list of discord member's income
    public List<GameObject> enemyList = new List<GameObject>(); //list of current enemies alive in the game

    private float minX;
    private float minY;
    private float maxX;
    private float maxY;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        //instantiateEnemy("EnemyCircle", "Enemy", 5);
        minX = gameArea.GetComponent<SpriteRenderer>().bounds.min.x;
        minY = gameArea.GetComponent<SpriteRenderer>().bounds.min.y;
        maxX = gameArea.GetComponent<SpriteRenderer>().bounds.max.x;
        maxY = gameArea.GetComponent<SpriteRenderer>().bounds.max.y;
        //for(int i = 0; i < 5; i++)
        //{
        //    instantiateEnemy(prefabs[0], getRandomPosition());
        //}
    }
	
	// Update is called once per frame
	void Update () {
        incrementEnemyTimeAlive();
        //addNewDiscordMembers();

    }

    /**
     * Use for discord call
    */
    public void instantiateEnemy(string enemyName, string userName, int spawnNumber)
    {
        GameObject prefab = getValue(enemyName);
        float spawnCost = spawnNumber * prefab.GetComponent<Enemy>().cost;

        //if(spawnCost < getMemberIncome(userName))
        //{
            for (int i = 0; i < spawnNumber; i++)
            {
                Vector2 position = getRandomPosition();
                GameObject newEnemy = (GameObject)Instantiate(prefab, position, Quaternion.identity);
                newEnemy.name = userName;
                //Random seperation distance
                newEnemy.GetComponent<Separation>().maxSepDist = Random.Range(0.1f, 2.1f);

                int leftHanded = Random.Range(0, 2);
                if(leftHanded == 0)
                {
                    Debug.Log("Left-handed!");
                    newEnemy.transform.Find("EnemySwordPlaceholdLeftHand").gameObject.SetActive(true);
                    newEnemy.transform.Find("EnemySwordPlacehold").gameObject.SetActive(false);
                } else
                {
                    Debug.Log("Right-handed!");
                    newEnemy.transform.Find("EnemySwordPlaceholdLeftHand").gameObject.SetActive(false);
                    newEnemy.transform.Find("EnemySwordPlacehold").gameObject.SetActive(true);
                }
                enemyList.Add(newEnemy);
            }
        //} else
        //{
        //    //not enough money
        //}
    }
    
    /*
     * returns the members current income
     */ 
    public float getMemberIncome(string userName)
    {
        return enemyIncome.First<EnemyIncome>(s => s.key.Equals(userName)).value;
    }

    /**
     * For testing purposes
    */
    public void instantiateEnemy(GameObject prefab, Vector2 position)
    {
        GameObject newEnemy = (GameObject)Instantiate(prefab, position, Quaternion.identity);

        newEnemy.GetComponent<Separation>().maxSepDist = Random.Range(0.1f, 2.1f);

        int leftHanded = Random.Range(0, 2);
        if (leftHanded == 0)
        {
            Debug.Log("Left-handed!");
            newEnemy.transform.Find("EnemySwordPlaceholdLeftHand").gameObject.SetActive(true);
            newEnemy.transform.Find("EnemySwordPlacehold").gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Right-handed!");
            newEnemy.transform.Find("EnemySwordPlaceholdLeftHand").gameObject.SetActive(false);
            newEnemy.transform.Find("EnemySwordPlacehold").gameObject.SetActive(true);
        }

        enemyList.Add(newEnemy);
    }

    /*
     * Generates a point on a circle around the player.
     * 
     * This is going to cause issues with enemies spawning outside of the play area
     * Possible fixes are if the enemy is spawned outside the play area, do a check and then have them spawn at a specific point
     */ 
    public Vector2 getRandomPosition()
    {
        Transform location = player.transform;

        float ang = Random.value * 360;
        float radius = Random.Range(10f, 15f);
        Vector2 pos = Vector2.zero;
        pos.x = location.transform.position.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = location.transform.position.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        while (pos.x < minX || pos.x > maxX 
            || pos.y < minY || pos.y > maxY)
        {
            ang = Random.value * 360;
            pos.x = location.transform.position.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
            pos.y = location.transform.position.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);

        }

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

    public void addNewDiscordMembers()
    {
        //Add new users to the income list
        foreach (DiscordMember dm in DiscordChatActor.Instance.Members)
        {
            EnemyIncome userIncome;
            userIncome = enemyIncome.First<EnemyIncome>(s => s.key.Equals(dm.Username));
            if (userIncome == null)
            {
                userIncome.key = dm.Username;
                userIncome.value = 0;
            }
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
        //Debug.Log(enemyDictionary.Count);
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

//The "dictionary" for holding the enemy name and it's prefab
[System.Serializable]
public class EnemyDictionary
{
    public string key;
    public GameObject value;

}

//The "dictionary" for hold the member's username and the amount of income they have
[System.Serializable]
public class EnemyIncome
{
    public string key; //username
    public float value = 0f; //income

    //Constructor
    EnemyIncome(string key)
    {
        this.key = key;
    }

    EnemyIncome(string key, float value)
    {
        this.key = key;
        this.value = value;
    }

}
