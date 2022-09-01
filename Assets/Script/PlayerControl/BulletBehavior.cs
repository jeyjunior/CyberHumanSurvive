using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{

    Rigidbody rb;
    public float moveSpeed = 1;

    public GameObject explosion;
    public bool move;
    public int bulletDMG = 5;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(this.gameObject, 3f);
    }

    void FixedUpdate()
    {
        //if (move) rb.MovePosition(rb.position + transform.forward * moveSpeed);
        if(move) rb.MovePosition(rb.position  - transform.up * moveSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
        Instantiate(explosion, transform.position, Quaternion.identity);

        if (collision.gameObject.CompareTag("Enemy")) 
            collision.gameObject.GetComponent<EnemyAttributes>().TakeDMG(bulletDMG);
    }
}
