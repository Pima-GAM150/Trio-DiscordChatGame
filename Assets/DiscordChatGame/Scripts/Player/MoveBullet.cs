using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour {

    public GameObject spawnPoint;
    public float speed = 100f;
    public float lifeSpan = 2f;
    public int dmg = 1;
    public Rigidbody2D rb;
    public Vector3 mousePosition;

    private float lifeTime;
    

    // Use this for initialization
    void Start () {
        Debug.Log(Input.mousePosition);
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

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
            Vector3 desired = mousePosition - this.transform.position;

            //normalize
            desired.Normalize();
            this.transform.position += desired / speed;
        }


        //Might want to change this to use a rigidbody and add velocity
    }

}
