using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public GameObject healthbar;
    public GameObject heartPrefab;
    public GameObject player;
    public List<GameObject> hearts;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(hearts.Count < player.GetComponent<PlayerController>().hp)
        {
            instaniateHealth();
        }
        if(hearts.Count > player.GetComponent<PlayerController>().hp) {
            destroyHeart();
        }  
    }

    public void instaniateHealth()
    {
        GameObject newHeart = (GameObject)Instantiate(heartPrefab, healthbar.transform.position, healthbar.transform.rotation);
        newHeart.transform.SetParent(healthbar.transform);
        hearts.Add(newHeart);
    }

    public void destroyHeart()
    {
        if(hearts.Count > 0)
        {
            Destroy(hearts[hearts.Count - 1]);
        }
    }
}
