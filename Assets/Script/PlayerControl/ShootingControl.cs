using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingControl : MonoBehaviour
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

            Vector3 currentEulerAngle = transform.localEulerAngles;
            currentEulerAngle.x = -90f;
            Instantiate(bullet, spawnPosition.position, Quaternion.Euler(currentEulerAngle));
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
