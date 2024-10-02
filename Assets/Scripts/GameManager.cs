using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public CharStats[] playerStats;

    public bool gameMenuOpen, dialogActive, fadingBetweenAreas;
    

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
        if(gameMenuOpen || dialogActive || fadingBetweenAreas)
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
}
