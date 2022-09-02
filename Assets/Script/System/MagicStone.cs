using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicStone : MonoBehaviour
{

    //Se o player não interagir com ela, ativado é falso
    public bool activated;
    bool particlesSpawned;

    public GameObject[] vfx;
    public MeshRenderer meshRendererMaterial;
    GameController gameController;

    float timeToChangeColor = 0;

    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        meshRendererMaterial.material.color = new Color(0, 0, 0);
    }

    private void FixedUpdate()
    {

        ChangeStoneColor();

    }


    void ChangeStoneColor()
    {
        //White
        if (activated && timeToChangeColor < 1)
        {
            timeToChangeColor += Time.deltaTime;
            meshRendererMaterial.material.color = new Color(timeToChangeColor, timeToChangeColor, timeToChangeColor);
        }

        //Black
        if (!activated && timeToChangeColor > 0)
        {
            timeToChangeColor -= Time.deltaTime;
            meshRendererMaterial.material.color = new Color(timeToChangeColor, timeToChangeColor, timeToChangeColor);
        }
    }

    public void ActiveMagicStone()
    {
        if(!activated && gameController.conqueredWaves < gameController.maxWave)
        {
            activated = true;
            SpawnParticles();
            gameController.spawnNextWave = true;
        }
    }

    void SpawnParticles()
    {
        Instantiate(vfx[0], transform.position, Quaternion.identity);
        Instantiate(vfx[1], new Vector3(transform.position.x, -1, transform.position.z), Quaternion.Euler(-90, transform.rotation.y, transform.rotation.z));
        Instantiate(vfx[2], transform.position, Quaternion.identity);

    }
    void PlaySound()
    {
        //Tocar algum som de conquista
    }

}
