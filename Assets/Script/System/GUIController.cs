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

    [Header("Text Win and GameOver")]
    public TMP_Text txtMaxKillLoose;
    public TMP_Text txtMaxKillWin;

    [Header("Text Instruction")]
    public GameObject instruction;

    [Header("Controle de volume")]
    public AudioSource[] audioSources;
    //0: PlayerMove, 1: PlayerShoot, 2: Enemy, 3: MagicStone, 4: GUI
    public AudioClip[] clips;
    //0: BtnSound, 1: GameMusic, 2: Victory, 3: GameOver

    private void Start()
    {
        gameController = GetComponent<GameController>();
        lifeBar.maxValue = gameController.life;
        lifeBar.value = gameController.life;
        ActivePanels("PanelInitialMenu");
        TimeScaleControl(0);

        //Volume no 100%
        volume.value = 1f;

        instruction.SetActive(false);
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
    public void ActivePanels(string name)
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

    #region Set Volume
    public void VolumeControl()
    {
        foreach(AudioSource audio in audioSources)
        {
            audio.volume = volume.value;

            if(audio == audioSources[2])
            {
                if(audio.volume > 0.5f)
                {
                    audio.volume = 0.5f;
                }
            }
        }

        
    }
    #endregion

    public void BtnPlaySound()
    {
        audioSources[4].PlayOneShot(clips[0]);
    }
}
