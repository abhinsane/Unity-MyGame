using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameMenu : MonoBehaviour
{

    [Header ("Menu Details")]
    public GameObject theMenu;

    public GameObject[] windows;
    private CharStats[] PlayerStats;

    public TextMeshProUGUI[] nameText, hpText, mpText,lvlText, expText;
    public Slider[] expSlider;
    public Image[] charimage;
    public GameObject[] charStatHolder;

    public GameObject[] statusButtons;

    
    public TextMeshProUGUI statusName,statusHP,statusMP,statusStrength,statusDefence,statusEquippedArmor,statusWeaponPower,statusEquippedWeapon,statusArmorPower,statusExpToNextlvl;
    public Image statusImage;

    [Header("Menu Button")]
    public ItemButton[] itemButtons;
    public string SelectedItem;
    public Item activeItem;
    public TextMeshProUGUI itemName,iteamDescription, useButtonText;

    public static GameMenu instance;

    public GameObject itemCharChoiceMenu;
    public TextMeshProUGUI[] itemCharChoiceNames;

    public TextMeshProUGUI[] goldText;

    public string mainMenuName;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
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

            AudioManager.instance.PlaySFX(5);
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
        for (int i = 0; i < goldText.Length; i++)
        {
            goldText[i].text = GameManager.instance.currentGold.ToString() + "g";
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
        itemCharChoiceMenu.SetActive(false);
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
        statusStrength.text = PlayerStats[Selected].Strength.ToString();
        statusDefence.text = PlayerStats[Selected].Defence.ToString();
        if(PlayerStats[Selected].EquippedWeapon != "")
        {
            statusEquippedWeapon.text = PlayerStats[Selected].EquippedWeapon;
        }
        else{
            statusEquippedWeapon.text = "None";
        }
        statusEquippedWeapon.text = PlayerStats[Selected].WeaponPower.ToString();
        if(PlayerStats[Selected].EquippedWeapon != "")
        {
            statusEquippedWeapon.text = PlayerStats[Selected].EquippedArmor;
        }
        else{
            statusEquippedArmor.text = "None";
        }
        statusArmorPower.text = PlayerStats[Selected].ArmorPower.ToString();
        statusExpToNextlvl.text = (PlayerStats[Selected].expToNextLevel[PlayerStats[Selected].PlayerLevel] - PlayerStats[Selected].currentEXP).ToString();
        statusImage.sprite = PlayerStats[Selected].charImage;
    }

    public void ShowItems()
    {

        GameManager.instance.SortItems();
        for(int i = 0; i < itemButtons.Length;i++)
        {
            itemButtons[i].buttonValue = i;

            if(GameManager.instance.itemHeld[i] != "")
            {
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemHeld[i]).itemSprite;
                itemButtons[i].amountText.text = GameManager.instance.NumberOfItems[i].ToString();

            }
            else
            {
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectItem(Item newItem)
    {
        activeItem = newItem;

        if(activeItem.isItem)
        {
            useButtonText.text = "Use";
        }
        if(activeItem.isWeapon || activeItem.isArmour)
        {
            useButtonText.text = "Equip";
        }

        itemName.text = activeItem.itemName;
        iteamDescription.text = activeItem.description;
    }

    public void DiscardItem()
    {
        if(activeItem !=null)
        {
            GameManager.instance.RemoveItem(activeItem.itemName);
        }
    }

    public void OpenItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(true);

        for(int i = 0; i < itemCharChoiceNames.Length;i++)
        {
            itemCharChoiceNames[i].text = GameManager.instance.playerStats[i].CharName;
            itemCharChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.instance.playerStats[i].gameObject.activeInHierarchy);
        }

    }
    

    public void CloseItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(true);

    }

    public void UseItem(int selectedChar)
    {
        activeItem.Use(selectedChar);
        CloseItemCharChoice();
    }


    public void SaveGame()
    {
        GameManager.instance.SaveData();
        QuestManager.instance.SaveQuestData();
    }

    public void PlayButtonSound()
    {
        AudioManager.instance.PlaySFX(4);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(mainMenuName);

        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(gameObject);
    }
    
}