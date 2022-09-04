using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GUIController : MonoBehaviour
{
    GameController gameController;

    [Header("Life")]
    public Slider lifeBar;

    [Header("Bug Kill")]
    public TMP_Text txtMaxKill;
    public TMP_Text txtKillCount;    
    
    [Header("Ammo Count")]
    public TMP_Text txtAmmoCount;
    public TMP_Text txtAmmoCase;

    [Header("Magic Stone")]
    public int activatedStoneCount;
    public List<Image> magicStonesIcon;

    [Header("Panels")]
    public GameObject[] panels;
    public bool panelsIsActive;

    [Header("Btn")]
    public GameObject BtnPause;

    [Header("Game Difficulty")]
    public int difficulty; //0: Easy, 1: Normal, 2: Hard
    public string nameDifficulty;
    public TMP_Text txtDifficulty;


    [Header("Volume")]
    public TMP_Text txtVolume;
    public Slider volume;


    private void Start()
    {
        gameController = GetComponent<GameController>();
        lifeBar.maxValue = gameController.life;
        lifeBar.value = gameController.life;
        BtnActivePanels("PanelInitialMenu");
        TimeScaleControl(0);

        //Volume no 50%
        volume.value = 0.5f;
    }
    private void Update()
    {
        lifeBar.value = gameController.life;
        TextControl();
        MagicStoneIconsControl();
        DifficultyControl();


    }
    public void TimeScaleControl(float value)
    {
        Time.timeScale = value;

        if(value == 0)
        {
            BtnPause.SetActive(false);
        }
        else
        {
            BtnPause.SetActive(true);
        }
    }
    public void TextControl()
    {
        txtMaxKill.text = gameController.maxDeadEnemys.ToString();
        txtKillCount.text = gameController.enemyKill.ToString();

        txtAmmoCount.text = gameController.bulletToShoot.ToString();
        txtAmmoCase.text = gameController.bulletCase.ToString();

        txtVolume.text = "Volume: " + (volume.value * 100).ToString("F0") + "%";
    }
    public void MagicStoneIconsControl()
    {
        activatedStoneCount = gameController.conqueredWaves - 1;
        
        if(activatedStoneCount >= 0 && activatedStoneCount < 3)
        {
            foreach(Image img in magicStonesIcon)
            {
                if(img == magicStonesIcon[activatedStoneCount]) { 
                    img.enabled = true; 
                }
            }
        }
        else
        {
            for(int i = 0; i < 3; i++)
            {
                magicStonesIcon[i].enabled = false;
            }
        }
    }

    #region Play Game
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
    public void NewGame()
    {
        //Utilizando no newgame e no continue
       StartCoroutine(StartGameCount(1));
    }
    public void Continue()
    {
        StartCoroutine(StartGameCount(0));
    }
    IEnumerator StartGameCount(float delay)
    {
        yield return new WaitForSeconds(delay);
        //Espera 2 segundos antes de ativar o bullet, assim player não atira enquanto esta navegando nos menus
        panelsIsActive = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShooting>().shootingEnable = true;
    }

    #endregion

    #region Panels Control
    public void BtnActivePanels(string name)
    {
        foreach(GameObject panel in panels)
        {
            if (panel.name == name)
            {
                panel.SetActive(true);
                panelsIsActive = true;
            }
            else
            {
                panel.SetActive(false);
            }
        }
    }

    #endregion

    #region Game Difficulty
    public void SetGameDifficulty(bool addValue)
    {
        //addValue false = subtração / addValue true = adição

        if (!addValue)
        {
            difficulty--;
        }
        else
        {
            difficulty++;
        }
    }
    void DifficultyControl()
    {


        if (difficulty < 0)
        {
            difficulty = 2;
        }
        else if(difficulty > 2)
        {
            difficulty = 0;
        }

        switch (difficulty)
        {
            case 0:
                nameDifficulty = "Easy";
                break;
            case 1:
                nameDifficulty = "Normal";
                break;
            case 2:
                nameDifficulty = "Hard";
                break;
        }

        txtDifficulty.text = nameDifficulty;
    }
    #endregion

    #region Set Volume - Programar o valor do slider no volume dos audios
    public void VolumeControl()
    {
        //Configurar o audio conforme o valor do slider
    }
    #endregion

    #region Data
    public void SaveData()
    {
        //Salvar quando clicar no botão voltar
    }
    public void LoadData()
    {

    }
    #endregion
}
