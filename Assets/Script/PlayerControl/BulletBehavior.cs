using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{

    Rigidbody rb;
    public float moveSpeed = 100;

    public GameObject explosion;
    public bool move;
    public int bulletDMG = 5;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(this.gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        if(move) rb.MovePosition(transform.position  -transform.up * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            Instantiate(explosion, transform.position, Quaternion.identity);

            if (collision.gameObject.CompareTag("Enemy")) collision.gameObject.GetComponent<EnemyAttributes>().TakeDMG(bulletDMG);
        }
    }
}
