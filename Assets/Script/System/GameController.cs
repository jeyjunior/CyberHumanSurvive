using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Space(5)]
    [Header("Wave Control")]
    public int conqueredWaves = 0;
    public int maxWave = 3;
    public bool spawnNextWave;

    [Space(5)]
    [Header("Enemy Control")]
    public int maxEnemyToSpawn = 30;
    public int enemySpawned = 0; //Contagem de quantos inimigos foram spawnados na wave
    public int enemyKill = 0; //Contagem de quantos inimigos foram mortos
    public int maxDeadEnemys; //Contagem total de inimigos mortos no game

    [Space(5)]
    [Header("Objs")]
    public MagicStone magicStone;
    public GameObject enemy;

    [Space(5)]
    [Header("Player Life Control")]
    public int life;
    public int maxLife;

    [Space(5)]
    [Header("Player Ammo Control")]
    public int amountAmmoCase = 3; //Quantas caixa de munição personagem tem
    public int amountOfAmmunition; //contagem de quantas balas restam no cartucho atual
    private int maxAmountAmmoPerCase = 80; //Quantas balas por case (usada para o reload)


    private void Start()
    {
        amountOfAmmunition = maxAmountAmmoPerCase;
    }

    private void Update()
    {
        if (!GetComponent<GUIController>().panelsIsActive)
        {
            if(conqueredWaves < maxWave)
            {
                if (spawnNextWave)
                {
                    SpawnEnemies();
                    spawnNextWave = false;
                }

                EnemyControl();
            }
        }
    }

    #region EnemyControl
    void EnemyControl()
    {
        if(enemyKill >= maxEnemyToSpawn)
        {
            conqueredWaves++;
            magicStone.activated = false;
            enemyKill = 0;
            enemySpawned = 0;
        }
    }
    public void MaxEnemyKill()
    {
        enemyKill++;
        maxDeadEnemys += enemyKill;
    }
    void SpawnEnemies()
    {
        for (int i = 0; i < maxEnemyToSpawn; i++)
        {
            int chooseSide = (int)Random.Range(1, 4);
            Instantiate(enemy, RandomPosition(chooseSide), Quaternion.identity);
            enemySpawned++;
        }
    }
    private Vector3 RandomPosition(int value)
    {
        switch (value)
        {
            case 1:
                //Esquerda
                return new Vector3(Random.Range(-19f, -10f), Random.Range(-5f, -3f), Random.Range(18f, -16f));
            case 2:
                //Baixo
                return new Vector3(Random.Range(-19f, 19f), Random.Range(-5f, -3f), Random.Range(-16f, -22f));            
            case 3:
                //Direita
                return new Vector3(Random.Range(8f, 20f), Random.Range(-5f, -3f), Random.Range(20f, -18f));
            default:
                return new Vector3(Random.Range(-19f, 19f), Random.Range(-5f, -3f), Random.Range(-16f, -22f));
        }
    }
    #endregion



}
