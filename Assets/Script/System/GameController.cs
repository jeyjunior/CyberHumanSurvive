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
    public bool winGame;

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
    public GameObject ammo;
    GameObject player;
    GUIController guiController;

    [Space(5)]
    [Header("Player Life Control")]
    public int life;
    public bool isDead;

    [Space(5)]
    [Header("Player Ammo Control")]
    private int maxBulletPerCase = 120; //Quantas balas por case (usada para o reload)
    public int bulletCase = 3; //Quantas caixa de muni??o personagem tem
    public int bulletToShoot; //contagem de quantas balas restam no cartucho atual
    public bool reload;

    

    private void Start()
    {
        bulletToShoot = maxBulletPerCase;
        player = GameObject.FindWithTag("Player");
        guiController = GetComponent<GUIController>();
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

        BulletControl();

        //gameOver
        if(life <= 0 && !isDead)
        {
            guiController.txtMaxKillLoose.text = $"Max kill: {maxDeadEnemys}";

            guiController.ActivePanels("PanelLooseGame");
            guiController.TimeScaleControl(0);
            isDead = true;
        }
        
        //win
        if(conqueredWaves >= maxWave && !winGame)
        {
            guiController.txtMaxKillWin.text = $"Max kill: {maxDeadEnemys}";

            guiController.ActivePanels("PanelWinGame");
            guiController.TimeScaleControl(0);
            winGame = true;

        }
    }
    public void TakeDmg(int dmg)
    {
        life -= dmg;
    }
    public void Shot() {

        bulletToShoot--;
    }
    void BulletControl()
    {
        if (bulletToShoot <= 0)
        {
            bulletCase--;
            bulletToShoot = maxBulletPerCase;
        }

        if (bulletCase < 0)
        {
            player.GetComponent<PlayerShooting>().shootingEnable = false;
            bulletCase = 0;
            bulletToShoot = 0;
        }
        else
        {
            player.GetComponent<PlayerShooting>().shootingEnable = true;
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

    //EnemyAttributes ira fazer a contagem quando inimigo morrer
    public void MaxEnemyKill()
    {
        enemyKill++;
        maxDeadEnemys++;
    }
    void SpawnEnemies()
    {
        for (int i = 0; i < maxEnemyToSpawn; i++)
        {
            //int chooseSide = (int)Random.Range(1, 4);
            Instantiate(enemy, new Vector3(Random.Range(-20,20), Random.Range(-10,-15), Random.Range(-20,20)), Quaternion.identity);
            enemySpawned++;
        }
    }
    /*
    private Vector3 RandomPosition(int value)
    {
        switch (value)
        {
            case 1:
                //Esquerda
                return new Vector3(Random.Range(-19f, -10f), Random.Range(-10f, -5f), Random.Range(18f, -16f));
            case 2:
                //Baixo
                return new Vector3(Random.Range(-19f, 19f), Random.Range(-10f, -5f), Random.Range(-16f, -22f));            
            case 3:
                //Direita
                return new Vector3(Random.Range(8f, 20f), Random.Range(-10f, -5f), Random.Range(20f, -18f));
            default:
                return new Vector3(Random.Range(-19f, 19f), Random.Range(-10f, -5f), Random.Range(-16f, -22f));
        }
    }
    */
    #endregion

    #region SpawnAmmo
    public void SpawnAmmo()
    {
        //Interagir com o pilar, spawna caixa de muni??o
        //Quantidade de muni??o ser? de acordo com a dificuldade
        for(int i = 0; i < GetComponent<GUIController>().difficulty + 1; i++)
        {
            Instantiate(ammo, new Vector3(Random.Range(-20, 20), -1, Random.Range(-20, 20)), Quaternion.identity);
        }
    }
    #endregion
}
