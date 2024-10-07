using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemButton : MonoBehaviour
{

    public Image buttonImage;
    public TextMeshProUGUI amountText;
    public int buttonValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Press()
    {
        if(GameMenu.instance.theMenu.activeInHierarchy)
        {

        
            if(GameManager.instance.itemHeld[buttonValue] != "")
            {
                GameMenu.instance.SelectItem(GameManager.instance.GetItemDetails(GameManager.instance.itemHeld[buttonValue]));
            }
        }

        if(Shop.instance.buyMenu.activeInHierarchy)
        {
            Shop.instance.SelecteBuyItem(GameManager.instance.GetItemDetails(Shop.instance.itemsForSale[buttonValue]));
        }

        if(Shop.instance.sellMenu.activeInHierarchy)
        {
            Shop.instance.SelectSellItem(GameManager.instance.GetItemDetails(GameManager.instance.itemHeld[buttonValue]));
        }
    }
}
