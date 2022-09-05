using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Obs")]
    GameController gameController;
    public GameObject bullet;
    public Transform spawnPosition;

    [Header("Shooting Control")]
    public bool isShooting;
    public float timeDelay;
    float timeCount = 0;
    public bool delaySpawnBullet;

    public bool spawnBullet;
    public bool shootingEnable = true; //Habilita tiro se o player tiver munição

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip pickupAmmon;


    private void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();

        //Volume Audio

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
        //Stop walk sound
        GetComponent<PlayerMovement>().PlayWalkSound(false);


        gameController.Shot();
        timeCount++;

        audioSource.Play(0);
        yield return new WaitForSeconds(timeDelay);
        Instantiate(bullet, spawnPosition.position, Quaternion.Euler(-90f, transform.localEulerAngles.y, transform.localEulerAngles.z));

        delaySpawnBullet = !delaySpawnBullet;
        timeCount = 0;
    }
    
    //Coletando munição
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BulletCase"))
        {
            Destroy(other.gameObject);
            gameController.bulletCase ++;
            audioSource.PlayOneShot(pickupAmmon);
        }
    }
}
