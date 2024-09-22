using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
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
        //update the information that is shown
        for(int i = 0; i <statusButtons.Length; i++)
        {
            statusButtons[i].SetActive(PlayerStats[i].gameObject.activeInHierarchy);
            statusButtons[i].GetComponentInChildren<Text>().text = PlayerStats[i].CharName;
        }
    }    
}
