using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    GameController gameController;
    public GameObject bullet;
    public Transform spawnPosition;

    public bool isShooting;
    public float timeDelay;
    float timeCount = 0;
    public bool delaySpawnBullet;

    public bool spawnBullet;
    public bool shootingEnable = true; //Habilita tiro se o player tiver muni��o

    private void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    void Update()
    {

        if (!gameController.GetComponent<GUIController>().panelsIsActive)
        {
            if (shootingEnable)
            {
                isShooting = Input.GetButton("Fire1");
                ShootingControl();
            }
            else
            {
                isShooting = false;
                StopCoroutine(DelaySpawnBullet());
            }
        }
    }

    //Atirar
    void ShootingControl()
    {
        if (isShooting && !delaySpawnBullet)
        {
            delaySpawnBullet = true;
        }

        if (delaySpawnBullet && timeCount == 0)
            StartCoroutine(DelaySpawnBullet());
    }

    //Delay para poder atirar novamente
    IEnumerator DelaySpawnBullet()
    {

        gameController.Shot();
        timeCount++;
        yield return new WaitForSeconds(timeDelay);
        Instantiate(bullet, spawnPosition.position, Quaternion.Euler(-90f, transform.localEulerAngles.y, transform.localEulerAngles.z));

        delaySpawnBullet = !delaySpawnBullet;
        timeCount = 0;
    }
    

    //Coletando muni��o
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BulletCase"))
        {
            Destroy(other.gameObject);
            gameController.bulletCase ++;
        }
    }
}
