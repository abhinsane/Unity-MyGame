using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{   


    public static Shop instance;

    public GameObject shopMenu;
    public GameObject buyMenu;
    public GameObject sellMenu;

    public TextMeshProUGUI goldText;

    public string[] itemsForSale;

    public ItemButton[] buyItemButtons;
    public ItemButton[] sellItemButtons;

    public Item selectedItem;
    public TextMeshProUGUI buyItemName, buyItemDescription, BuyItemValue;
    public TextMeshProUGUI sellItemName, sellItemDescription, sellItemValue;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K) && !shopMenu.activeInHierarchy)
        {
            OpenShop();
        }
    }

    public void OpenShop()
    {
        shopMenu.SetActive(true);
        OpenBuyMenu();

        GameManager.instance.shopActive = true;

        goldText.text = GameManager.instance.currentGold.ToString() + "g";

    }

    public void CloseShop()
    {
        shopMenu.SetActive(false);

        GameManager.instance.shopActive = false;

    }

    public void OpenBuyMenu()
    {

        buyItemButtons[0].Press();
        buyMenu.SetActive(true);
        sellMenu.SetActive(false);

        GameManager.instance.SortItems();
        for(int i = 0; i < buyItemButtons.Length;i++)
        {
            buyItemButtons[i].buttonValue = i;

            if(itemsForSale[i] != "")
            {
                buyItemButtons[i].buttonImage.gameObject.SetActive(true);
                buyItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(itemsForSale[i]).itemSprite;
                buyItemButtons[i].amountText.text = "";

            }
            else
            {
                buyItemButtons[i].buttonImage.gameObject.SetActive(false);
                buyItemButtons[i].amountText.text = "";
            }
        }
    }

    public void OpenSellMenu()
    {
        sellItemButtons[0].Press();
        buyMenu.SetActive(false);
        sellMenu.SetActive(true);

        ShowSellItems();
    }
    
    private void ShowSellItems()
    {
        GameManager.instance.SortItems();

        for(int i = 0; i < sellItemButtons.Length;i++)
        {
            sellItemButtons[i].buttonValue = i;

            if(GameManager.instance.itemHeld[i] != "")
            {
                sellItemButtons[i].buttonImage.gameObject.SetActive(true);
                sellItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemHeld[i]).itemSprite;
                sellItemButtons[i].amountText.text = GameManager.instance.NumberOfItems[i].ToString();

            }
            else
            {
                sellItemButtons[i].buttonImage.gameObject.SetActive(false);
                sellItemButtons[i].amountText.text = "";
            }
        }
    }
    public void SelecteBuyItem(Item buyItem)
    {
        selectedItem = buyItem;
        buyItemName.text = selectedItem.itemName;
        buyItemDescription.text = selectedItem.description;
        BuyItemValue.text = "Value: " + selectedItem.value + "g";
    }

    public void SelectSellItem(Item SellItem)
    {
        selectedItem = SellItem;
        sellItemName.text = selectedItem.itemName;
        sellItemDescription.text = selectedItem.description;
        sellItemValue.text = "Value: " + Mathf.FloorToInt(selectedItem.value * 0.5f).ToString() + "g"; 
    }

    public void BuyItem()
    {
        if(selectedItem != null)
        {
            if(GameManager.instance.currentGold >= selectedItem.value)
            {
                GameManager.instance.currentGold -= selectedItem.value;
                GameManager.instance.AddItem(selectedItem.itemName);
            }
            goldText.text = GameManager.instance.currentGold.ToString() + "g";
        }
    }    
    public void SellItem()
    {
        if(selectedItem != null)
        {
            GameManager.instance.currentGold += Mathf.FloorToInt(selectedItem.value * 0.5f); 
            GameManager.instance.RemoveItem(selectedItem.itemName);

        }
        goldText.text = GameManager.instance.currentGold .ToString() + "g";
        ShowSellItems();
    }
}