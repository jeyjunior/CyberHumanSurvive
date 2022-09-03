using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

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


    private void Start()
    {
        gameController = GetComponent<GameController>();
        lifeBar.maxValue = gameController.life;
        lifeBar.value = gameController.life;
    }
    private void Update()
    {
        lifeBar.value = gameController.life;
        TextControl();
        MagicStoneIconsControl();
    }
    public void TextControl()
    {
        txtMaxKill.text = gameController.maxDeadEnemys.ToString();
        txtKillCount.text = gameController.enemyKill.ToString();

        txtAmmoCount.text = gameController.bulletToShoot.ToString();
        txtAmmoCase.text = gameController.bulletCase.ToString();
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
    public void BtnActivePanels(string name)
    {
        foreach(GameObject panel in panels)
        {
            if (panel.name == name) panel.SetActive(true);
            else panel.SetActive(false);
        }
    }
    public void TimeScaleControl(float value)
    {
        Time.timeScale = value;

        if(panelsIsActive)
        {
            BtnPause.SetActive(false);
        }
        else
        {
            BtnPause.SetActive(true);
        }
    }
    public void SaveData()
    {
        //Salvar quando clicar no botão voltar
    }
}
