using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public CharStats[] playerStats;

    public bool gameMenuOpen, dialogActive, fadingBetweenAreas, shopActive, battleActive;
    

    public string[] itemHeld;
    public int[] NumberOfItems;
    public Item[] referenceItems;

    public int currentGold;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);

        SortItems();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameMenuOpen || dialogActive || fadingBetweenAreas || shopActive)
        {
            PlayerController.instance.canMove = false;
        }
        else
        {
            PlayerController.instance.canMove = true;

        }

        if(Input.GetKeyDown(KeyCode.J))
        {
            AddItem("Iron Armor");
            AddItem("Nigga");

            RemoveItem("Health potion");
            RemoveItem("Bitch");
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            SaveData();
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            LoadData();
        }
    

    }

    public Item GetItemDetails(string itemToGrab)
    {
        for(int i = 0; i<referenceItems.Length;i++)
        {
            if(referenceItems[i].itemName == itemToGrab)
            {
                return referenceItems[i];
            }
            
        }





        return null;
    }

    public void SortItems()
    {

        bool itemAfterSpace = true;
        while(itemAfterSpace)
        {

            itemAfterSpace = false;
            for(int i = 0; i<itemHeld.Length-1;i++)
            {
                if(itemHeld[i] == "")
                {
                    itemHeld[i] = itemHeld[i+1];
                    itemHeld[i+1] ="";

                    NumberOfItems[i] = NumberOfItems[i+1];
                    NumberOfItems[i+1] = 0;

                    if(itemHeld[i] != "")
                    {
                        itemAfterSpace = true; //like shifting _of_1  001000000    010000000     10000000
                    }
                }
            }
        }

    }

    public void AddItem(string itemToAdd)
        {
            int newItemPosition = 0;
            bool foundSpace = false;

            for(int i = 0; i< itemHeld.Length;i++)
            {
                if(itemHeld[i] == "" || itemHeld[i] == itemToAdd)
                {
                    newItemPosition = i;
                    i = itemHeld.Length;
                    foundSpace = true;
                }
            }

            if(foundSpace)
            {
                bool itemExists = false;
                for(int i = 0; i<referenceItems.Length;i++)
                {
                    if(referenceItems[i].itemName == itemToAdd)
                    {
                        itemExists = true;
                        i = referenceItems.Length;

                    }
                }
                if(itemExists)
                {
                    itemHeld[newItemPosition] = itemToAdd;
                    NumberOfItems[newItemPosition]++;

                }
                else
                {
                    Debug.LogError(itemToAdd + " Does not Exist!!");

                }

                GameMenu.instance.ShowItems();
            }
        }

        public void RemoveItem(string itemToRemove)
        {
           bool foundItem = false;
           int ItemPosition = 0;

           for(int i = 0; i< itemHeld.Length;i++)
           {
            if(itemHeld[i] == itemToRemove)
            {
                foundItem = true;
                ItemPosition = i;


                i = itemHeld.Length;

            }
           } 

           if(foundItem)
           {
                NumberOfItems[ItemPosition]--;

                if(NumberOfItems[ItemPosition]<= 0)
                {
                    itemHeld[ItemPosition] = "";
                }
                GameMenu.instance.ShowItems();
           }
           else
           {
            Debug.LogError("Couldn't find " + itemToRemove);
           }
        }

        public void SaveData()
        {
            PlayerPrefs.SetString("Current_Scene",SceneManager.GetActiveScene().name);
            PlayerPrefs.SetFloat("Player_position_x",PlayerController.instance.transform.position.x);
            PlayerPrefs.SetFloat("Player_position_y",PlayerController.instance.transform.position.y);
            PlayerPrefs.SetFloat("Player_position_z",PlayerController.instance.transform.position.z);


            //save character info
            for(int i = 0; i<playerStats.Length; i++)
            {
                if(playerStats[i].gameObject.activeInHierarchy)
                {
                    PlayerPrefs.SetInt("Player_" + playerStats[i].CharName + "_active", 1);
                }
                else
                {
                    PlayerPrefs.SetInt("Player_" + playerStats[i].CharName + "_active", 0);
                }
                PlayerPrefs.SetInt("Player_" + playerStats[i].CharName + "_level",playerStats[i].PlayerLevel);
                PlayerPrefs.SetInt("Player_" + playerStats[i].CharName + "_currentExp",playerStats[i].currentEXP);
                PlayerPrefs.SetInt("Player_" + playerStats[i].CharName + "_currentHP",playerStats[i].currentHP);
                PlayerPrefs.SetInt("Player_" + playerStats[i].CharName + "_MaxHP",playerStats[i].maxHP);
                PlayerPrefs.SetInt("Player_" + playerStats[i].CharName + "_CurrentMP",playerStats[i].currentMP);
                PlayerPrefs.SetInt("Player_" + playerStats[i].CharName + "_MaxMP",playerStats[i].maxMP);
                PlayerPrefs.SetInt("Player_" + playerStats[i].CharName + "_Strength",playerStats[i].Strength);
                PlayerPrefs.SetInt("Player_" + playerStats[i].CharName + "_Defence",playerStats[i].Defence);
                PlayerPrefs.SetInt("Player_" + playerStats[i].CharName + "_WeaponPower",playerStats[i].WeaponPower);
                PlayerPrefs.SetInt("Player_" + playerStats[i].CharName + "_ArmorPower",playerStats[i].ArmorPower);
                PlayerPrefs.SetString("Player_" + playerStats[i].CharName + "_EquippedWeapon",playerStats[i].EquippedWeapon);
                PlayerPrefs.SetString("Player_" + playerStats[i].CharName + "_EquippedArmor",playerStats[i].EquippedArmor);
                
            }
            //stores inventory data

            for(int i =0; i<itemHeld.Length;i++)
            {
                PlayerPrefs.SetString("ItemInInventory_"+ i, itemHeld[i]);
                PlayerPrefs.SetInt("ItemAmount_" + i, NumberOfItems[i]);
            }
        }

        public void LoadData()
        {
            PlayerController.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_position_x"),PlayerPrefs.GetFloat("Player_position_y"),PlayerPrefs.GetFloat("Player_position_z"));

            for(int i =0; i<playerStats.Length;i++)
            {
                if(PlayerPrefs.GetInt("Player_" + playerStats[i].CharName + "_active") ==0)
                {
                    playerStats[i].gameObject.SetActive(false);
                }
                else
                {
                    playerStats[i].gameObject.SetActive(true);
                }
                playerStats[i].PlayerLevel = PlayerPrefs.GetInt("Player_" + playerStats[i].CharName + "_level");
                playerStats[i].currentEXP = PlayerPrefs.GetInt("Player_" + playerStats[i].CharName + "_currentExp");
                playerStats[i].currentHP = PlayerPrefs.GetInt("Player_" + playerStats[i].CharName + "_currentHP");
                playerStats[i].maxHP = PlayerPrefs.GetInt("Player_" + playerStats[i].CharName + "_MaxHP");
                playerStats[i].currentMP = PlayerPrefs.GetInt("Player_" + playerStats[i].CharName + "_CurrentMP");
                playerStats[i].maxMP = PlayerPrefs.GetInt("Player_" + playerStats[i].CharName + "_MaxMP");
                playerStats[i].Strength = PlayerPrefs.GetInt("Player_" + playerStats[i].CharName + "_Strength");
                playerStats[i].Defence = PlayerPrefs.GetInt("Player_" + playerStats[i].CharName + "_Defence");
                playerStats[i].WeaponPower = PlayerPrefs.GetInt("Player_" + playerStats[i].CharName + "_WeaponPower");
                playerStats[i].ArmorPower = PlayerPrefs.GetInt("Player_" + playerStats[i].CharName + "_ArmorPower");
                playerStats[i].EquippedWeapon = PlayerPrefs.GetString("Player_" + playerStats[i].CharName + "_EquippedWeapon");
                playerStats[i].EquippedArmor = PlayerPrefs.GetString("Player_" + playerStats[i].CharName + "_EquippedArmor");
            }

            for(int i =0; i<itemHeld.Length;i++)
            {
                itemHeld[i] = PlayerPrefs.GetString("ItemInInventory_"+ i);
                NumberOfItems[i] = PlayerPrefs.GetInt("ItemAmount_" + i);
            }
        }
}
