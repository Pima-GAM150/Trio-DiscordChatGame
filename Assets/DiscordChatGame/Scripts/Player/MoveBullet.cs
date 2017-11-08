using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour {

    public PlayerController pc;  
    public GameObject spawnPoint;
    public float speed = 100f;
    public float lifeSpan = 2f;
    public int dmg = 1;
    public Rigidbody2D rb;
    public Vector3 mousePosition;

    private float lifeTime;
    private Vector3 desired;


    // Use this for initialization
    void Start () {
        //Debug.Log(Input.mousePosition);
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        desired = mousePosition - this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime > lifeSpan)
        {
            Destroy(this.gameObject);
        }
        Move();
    }

    public void Move()
    {
        //rb.velocity += Vector3.forward * speed * 10;
        if (this.gameObject != null)
        {
            //normalize
            desired.Normalize();
            this.transform.position += desired / speed;
        }


        //Might want to change this to use a rigidbody and add velocity
    }

    public void OnCollisionEnter2D(Collision2D collider)
    {
        Debug.Log(collider.gameObject.name);
        if(collider.gameObject.tag.Contains("Enemy"))
        {
            collider.gameObject.GetComponent<Enemy>().hp -= pc.dmg;
            collider.gameObject.GetComponent<EnemyController>().onDeath();
            //pc.currentIncome += collider.gameObject.GetComponent<Enemy>().cost;
            Destroy(this.gameObject);
        }
        if(collider.gameObject.name.Contains("Wall"))
        {
            Destroy(this.gameObject);
        }
    }

}
