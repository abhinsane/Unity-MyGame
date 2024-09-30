using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Type")]
    public bool isItem;
    public bool isWeapon;
    public bool isArmour;

    [Header("Item Details")]
    public string itemName;
    public string description;
    public int value;
    public Sprite itemSprite;

    [Header ("Item Details")]
    public int amountToChange;
    public bool affectHp, affectMp, affectStr;

    [Header("Weapon/Armor Details")]
    public int weaponStrength;
    public int armorStrength;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use(int charToUseOn)
    {
        CharStats selectedChar = GameManager.instance.playerStats[charToUseOn];

        if(isItem)
        {
            if(affectHp)
            {
                selectedChar.currentHP += amountToChange;

                if(selectedChar.currentHP > selectedChar.maxHP)
                {
                    selectedChar.currentHP = selectedChar.maxHP;
                }
            }

            if(affectMp)
            {
                selectedChar.currentMP += amountToChange;

                if(selectedChar.currentMP > selectedChar.maxMP)
                {
                    selectedChar.currentMP = selectedChar.maxMP;
                }
            }

            if(affectStr)
            {
                selectedChar.Strength += amountToChange;
            }

            if(isWeapon)
            {
                if(selectedChar.EquippedWeapon != "")
                {
                    GameManager.instance.AddItem(selectedChar.EquippedWeapon);
                }
                selectedChar.EquippedWeapon = itemName;
                selectedChar.WeaponPower = weaponStrength;

            }

            if(isArmour)
            {
                if(selectedChar.EquippedWeapon != "")
                {
                    GameManager.instance.AddItem(selectedChar.EquippedArmor);
                }
                selectedChar.EquippedArmor = itemName;
                selectedChar.ArmorPower = armorStrength;

            }
        }
        GameManager.instance.RemoveItem(itemName);
    }
}
