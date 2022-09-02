using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform spawnPosition;

    public bool isShooting;
    public float timeDelay;
    float timeCount = 0;
    public bool delaySpawnBullet;

    public bool spawnBullet;

    // Update is called once per frame
    void Update()
    {
        isShooting = Input.GetButton("Fire1");

        if (isShooting && ! delaySpawnBullet)
        {
            delaySpawnBullet = true;

            //Modelo simples:
            //Instantiate(bullet, spawnPosition.transform.position, spawnPosition.transform.rotation);

            Instantiate(bullet, spawnPosition.position, Quaternion.Euler(-90f, transform.localEulerAngles.y, transform.localEulerAngles.z));
        }

        if (delaySpawnBullet && timeCount == 0) 
            StartCoroutine(DelaySpawnBullet());
    }

    IEnumerator DelaySpawnBullet()
    {
        timeCount++;
        yield return new WaitForSeconds(timeDelay);
        delaySpawnBullet = !delaySpawnBullet;
        timeCount = 0;
    }
}
