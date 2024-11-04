using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleItemSelect : MonoBehaviour
{

    public string itemName;
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

    public void press()
    {
        if(BattleManager.instance.itemMenu.activeInHierarchy)
        {
            if(GameManager.instance.itemHeld[buttonValue]!= "")
            {
                BattleManager.instance.SelectedItem(GameManager.instance.GetItemDetails(GameManager.instance.itemHeld[buttonValue]));

            }
        }
    }
}
