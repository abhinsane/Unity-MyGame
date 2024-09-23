using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameMenu : MonoBehaviour
{

    public GameObject theMenu;

    public GameObject[] windows;
    private CharStats[] PlayerStats;

    public TextMeshProUGUI[] nameText, hpText, mpText,lvlText, expText;
    public Slider[] expSlider;
    public Image[] charimage;
    public GameObject[] charStatHolder;

    public GameObject[] statusButtons;

    public TextMeshProUGUI statusName,statusHP,statusMP,statusStr,statusDef,statusWpnEqpd,statusWpnPwr,statusArmrEqpd,statusArmrPwr,statusExp;
    public Image statusImage;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire2"))
        {
            if(theMenu.activeInHierarchy)
            {
                theMenu.SetActive(false);
                UpdateMainStats();
                //GameManager.instance.gameMenuOpen = false;

                CloseMenu();
            }
            else
            {
                theMenu.SetActive(true);
                GameManager.instance.gameMenuOpen = true;
            }
        }
    }

    public void UpdateMainStats()
    {
        PlayerStats = GameManager.instance.playerStats;

        for(int i = 0; i < PlayerStats.Length;i++)
        {
            if(PlayerStats[i].gameObject.activeInHierarchy)
            {
                charStatHolder[i].SetActive(true);

                nameText[i].text = PlayerStats[i].CharName;
                hpText[i].text = "Hp: " + PlayerStats[i].currentHP + "/" + PlayerStats[i].maxHP;
                mpText[i].text = "Mp: " + PlayerStats[i].currentMP + "/" + PlayerStats[i].maxMP;
                lvlText[i].text ="Lvl: " + PlayerStats[i].PlayerLevel + "/" + PlayerStats[i].PlayerLevel;
                expText[i].text = "" + PlayerStats[i].currentEXP + "/" + PlayerStats[i].expToNextLevel[PlayerStats[i].PlayerLevel];
                expSlider[i].maxValue = PlayerStats[i].expToNextLevel[PlayerStats[i].PlayerLevel];
                expSlider[i].value = PlayerStats[i].currentEXP;
                charimage[i].sprite = PlayerStats[i].charImage;
            }
            else
            {
                charStatHolder[i].SetActive(false);
            }
        }

    }

    public void ToggleWindow(int windowNumber)
    {
        UpdateMainStats();
        for(int i = 0; i < windows.Length; i++)
        {
            if(i==windowNumber)
            {
            windows[i].SetActive(!windows[i].activeInHierarchy);
            }
            else
            {
                windows[i].SetActive(false);
            }
        }
    }

    public void CloseMenu()
    {
        for(int i = 0;i < windows.Length;i++)
        {
            windows[i].SetActive(false);
        
        }
        theMenu.SetActive(false);

        GameManager.instance.gameMenuOpen = false;
    }

    public void OpenStatus()
    {
        UpdateMainStats();
        StatusChar(0);
 
        for(int i = 0; i<statusButtons.Length; i++)
        {
            statusButtons[i].SetActive(PlayerStats[i].gameObject.activeInHierarchy);
            statusButtons[i].GetComponentInChildren<TextMeshProUGUI>(true).text = PlayerStats[i].CharName;
        }
    }

    public void StatusChar(int Selected)
    {
        statusName.text = PlayerStats[Selected].CharName;
        statusHP.text = PlayerStats[Selected].currentHP + "/" +PlayerStats[Selected].maxHP;
        statusMP.text = PlayerStats[Selected].currentMP + "/" + PlayerStats[Selected].maxMP;
        statusStr.text = PlayerStats[Selected].strength.ToString();
        statusDef.text = PlayerStats[Selected].defence.ToString();
        if(PlayerStats[Selected].equippedWpn != "")
        {
            statusWpnEqpd.text = PlayerStats[Selected].equippedWpn;
        }
        statusWpnEqpd.text = PlayerStats[Selected].wpnPwr.ToString();
        if(PlayerStats[Selected].equippedWpn != "")
        {
            statusWpnEqpd.text = PlayerStats[Selected].equippedArmr;
        }
        statusArmrPwr.text = PlayerStats[Selected].armPwr.ToString();
        statusExp.text = (PlayerStats[Selected].expToNextLevel[PlayerStats[Selected].PlayerLevel] - PlayerStats[Selected].currentEXP).ToString();
        statusImage.sprite = PlayerStats[Selected].charImage;
    }
}