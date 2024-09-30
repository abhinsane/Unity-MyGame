using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharStats : MonoBehaviour
{

    public string CharName;
    public int PlayerLevel = 1;
    public int currentEXP;
    public int[] expToNextLevel;
    public int maxLevel = 100;
    public int baseEXP = 1000;


    public int currentHP;
    public int maxHP = 100;

    public int[] mpLvlBonus;
    public int currentMP;
    public int maxMP = 30;
    public int Strength;
   public int Defence;
   public int WeaponPower;
   public int ArmorPower;
   public string EquippedWeapon;
   public string EquippedArmor;
   public Sprite charImage;


    // Start is called before the first frame update
    void Start()
    {
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseEXP;

        for(int i = 2; i<expToNextLevel.Length;i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i-1] * 1.05f);

        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            AddExp(1000);
        }
    }

    public void AddExp(int expToAdd)
    {
        currentEXP += expToAdd;

        if(PlayerLevel < maxLevel)
        {
            if(currentEXP > expToNextLevel[PlayerLevel])
            {
                currentEXP -= expToNextLevel[PlayerLevel];

                PlayerLevel++;

                //determine wheter to add to str or def based on odd or even 

                if(PlayerLevel%2 == 0)
                {
                    Strength++;
                }
                else
                {
                    Defence++;
                }

                maxHP = Mathf.FloorToInt(maxHP *1.05f);
                currentHP = maxHP;


                maxMP += maxMP +mpLvlBonus[PlayerLevel];
                currentMP = maxMP;


            }

            if(PlayerLevel >= maxLevel)
            {
                currentEXP = 0;
            }
        }    
    }
}
