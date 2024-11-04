using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{

    public static BattleManager instance;

    private bool battleActive;
    public GameObject battleScene;
    public Transform[] playerPositions;
    public Transform[] enemyPositions;
    public BattleChar[] playerPrefabs;
    public BattleChar[] enemyPrefabs;
    public List<BattleChar>activeBattlers = new List<BattleChar>();

    public int currentTurn;
    public bool turnWaiting;
    public GameObject uiButtonsHolder;
    public BattleMove[] moveList;
    public GameObject enemyAttackEffect;
    public DamageNumber theDamageNumber;
    public TextMeshProUGUI[] playerName, playerHP, playerMP;

    public GameObject targetMenu;
    public BattleTargetButton[] targetButtons;
    public GameObject magicMenu;
    public BattleMagicSelect[] magicButtons;
    public BattleNotification battleNotice;

    public string SelectItem;
    public GameObject itemMenu;
    public BattleItemSelect[] itemButtons;
    public Item activeItem;
    public TextMeshProUGUI itemName, itemDescription, useButtonText;
    public GameObject itemCharChoiceMenu;
    public TextMeshProUGUI[] itemCharChoiceNames;
    
    public int chanceToFLee = 35;

    public string gameOverScene;
    private bool fleeing;

    public int rewardXP;
    public string[] rewardItems;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            battleStart(new string[] {"Eyeball","Gobbler","Slime"});

        }

        if(battleActive)
        {
            if(turnWaiting)
            {
                if(activeBattlers[currentTurn].isPlayer)
                {
                    uiButtonsHolder.SetActive(true);
                }
                else
                {
                    uiButtonsHolder.SetActive(false);

                    //enemy should attack
                    StartCoroutine(EnemyMoveCo());

                }

                if(Input.GetKeyDown(KeyCode.N))
                {
                    NextTurn();
                }
            }
        }
    }

    public void battleStart(string[] enemiesToSpawn)
    {
        if(!battleActive)
        {
            battleActive = true;

            GameManager.instance.battleActive = true;

            transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y,transform.position.z);
            battleScene.SetActive(true);

            AudioManager.instance.PlayBGM(6);

            for(int i = 0;i<playerPositions.Length;i++)
            {
                if(GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
                {
                    for(int j = 0; j<playerPrefabs.Length; j++)
                    {
                        if(playerPrefabs[j].charName == GameManager.instance.playerStats[i].CharName)
                        {
                            BattleChar newPlayer = Instantiate(playerPrefabs[j],playerPositions[i].position,playerPositions[i].rotation);
                            newPlayer.transform.parent = playerPositions[i];
                            activeBattlers.Add(newPlayer);

                            CharStats thePlayer = GameManager.instance.playerStats[i];

                            activeBattlers[i].currentHP = thePlayer.currentHP;  
                            activeBattlers[i].maxHP = thePlayer.maxHP;
                            activeBattlers[i].currentMP = thePlayer.currentMP;
                            activeBattlers[i].maxMp = thePlayer.maxMP;
                            activeBattlers[i].Strength = thePlayer.Strength;
                            activeBattlers[i].Defence = thePlayer.Defence;
                            activeBattlers[i].weaponPower = thePlayer.WeaponPower;
                            activeBattlers[i].armorPower = thePlayer.ArmorPower;

                        }
                    }
                }
            }

            for(int i = 0;i<enemiesToSpawn.Length;i++)
            {
                if(enemiesToSpawn[i] != "")
                {
                    for(int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if(enemyPrefabs[j].charName == enemiesToSpawn[i])
                        {
                            BattleChar newEnemy = Instantiate(enemyPrefabs[j],enemyPositions[i].position,enemyPositions[i].rotation);
                            newEnemy.transform.parent = enemyPositions[i];
                            activeBattlers.Add(newEnemy);
                        }
                    }
                }
            }

            turnWaiting = true;
            currentTurn = Random.Range(0,activeBattlers.Count);

            UpdateUIStats();
        }
    }

    public void NextTurn()
    {
        currentTurn++;
        if(currentTurn>= activeBattlers.Count)
        {
            currentTurn = 0;
        }
        turnWaiting = true;

        Updatebattle();
        UpdateUIStats();

    }

    public void Updatebattle()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        for(int i = 0; i<activeBattlers.Count; i++)
        {
            if(activeBattlers[i].currentHP<0)
            {
                activeBattlers[i].currentHP = 0;

            }

            if(activeBattlers[i].currentHP == 0)
            {
                //handle dead battler
                if(activeBattlers[i].isPlayer)
                {
                    activeBattlers[i].theSprite.sprite = activeBattlers[i].deadSprite;
                }
                else
                {
                    activeBattlers[i].EnemyFade();
                }
            }
            else
            {
                if(activeBattlers[i].isPlayer)
                {
                    allPlayersDead = false;
                    activeBattlers[i].theSprite.sprite = activeBattlers[i].aliveSprite;

                }
                else
                {
                    allEnemiesDead = false;
                }
            }
        }
        if(allEnemiesDead || allPlayersDead)
        {
            if(allEnemiesDead)
            {
                //end battle in victory
                StartCoroutine(EndBattleCo());
            }
            else
            {
                //end battle in failure
                StartCoroutine(GameOverCo());
            }

            /* battleScene.SetActive(false);
            GameManager.instance.battleActive = false;
            battleActive = false; */
        }
        else
        {
            while(activeBattlers[currentTurn].currentHP == 0)
            {
                currentTurn++;
                if(currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0;
                }
            }
        }
    }


    public IEnumerator EnemyMoveCo()
    {
        turnWaiting = false;
        yield return new WaitForSeconds(1f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        NextTurn();
    }
    public void EnemyAttack()
    {
        List<int>players = new List<int>();
        for(int i=0; i<activeBattlers.Count;i++) 
        {
            if(activeBattlers[i].isPlayer && activeBattlers[i].currentHP >0)
            {
                players.Add(i);
            }
        }

        int selectedTarget = players[Random.Range(0,players.Count)];


        //activeBattlers[selectedTarget].currentHP -= 30;   

        int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Length);
        int movePower = 0;
        for(int i=0; i<moveList.Length;i++)
        {
            if(moveList[i].moveName ==activeBattlers[currentTurn].movesAvailable[selectAttack])
            {
                Instantiate(moveList[i].theEffect,activeBattlers[selectedTarget].transform.position,activeBattlers[selectedTarget].transform.rotation);
                movePower = moveList[i].movePower;
            }
        } 

        Instantiate(enemyAttackEffect,activeBattlers[currentTurn].transform.position,activeBattlers[currentTurn].transform.rotation);
        
        DealDamage(selectedTarget,movePower);
    }

    public void DealDamage(int target, int movePower)
    {
        float attackPower = activeBattlers[currentTurn].Strength + activeBattlers[currentTurn].weaponPower;
        float defencePower = activeBattlers[currentTurn].Defence + activeBattlers[target].armorPower;

        float damagecalc = (attackPower/defencePower + 5 ) * Random.Range(.9f,1.5f);
        int damageToGive = Mathf.RoundToInt(damagecalc);

        Debug.Log(activeBattlers[currentTurn].charName + " is dealing" +damagecalc + "(" + damageToGive + ") damage to " + activeBattlers[target].charName);

       if(activeBattlers[target].isPlayer)
       {
        CharStats selectedChar = GameManager.instance.playerStats[target];
        selectedChar.currentHP -= damageToGive;
       }
       if(!activeBattlers[target].isPlayer)
       {
        activeBattlers[target].currentHP -= damageToGive;
       }

        Instantiate(theDamageNumber, activeBattlers[target].transform.position,activeBattlers[target].transform.rotation).SetDamage(damageToGive);

        UpdateUIStats();

    }

    public void UpdateUIStats()
    {
        for(int i =0; i< playerName.Length;i++)
        {
            if(activeBattlers.Count >i)
            {
                if(activeBattlers[i].isPlayer)
                {
                    BattleChar playerData = activeBattlers[i];


                    playerName[i].gameObject.SetActive (true);
                    playerName[i].text = playerData.charName;
                    playerHP[i].text = Mathf.Clamp(playerData.currentHP, 0, int.MaxValue) + "/" + playerData.maxHP;
                    playerMP[i].text = Mathf.Clamp(playerData.currentMP, 0, int.MaxValue) + "/" + playerData.maxMp;

                }
                else
                {
                    playerName[i].gameObject.SetActive(false);
                }
            }
            else
            {
                playerName[i].gameObject.SetActive(false);
            }
        }
    }

    public void PlayerAttack(string moveName, int selectedTarget)
    {
       
        int movePower = 0;
        for(int i=0; i<moveList.Length;i++)
        {
            if(moveList[i].moveName == moveName)
            {
                Instantiate(moveList[i].theEffect,activeBattlers[selectedTarget].transform.position,activeBattlers[selectedTarget].transform.rotation);
                movePower = moveList[i].movePower;
            }
        }

        Instantiate(enemyAttackEffect,activeBattlers[currentTurn].transform.position,activeBattlers[currentTurn].transform.rotation);

        DealDamage(selectedTarget,movePower); 

        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);

        NextTurn();

    }


    public void OpenTargetMenu(string moveName)
    {
        targetMenu.SetActive(true);

        List<int> Enemies = new List<int>();
        for(int i=0; i < activeBattlers.Count; i++)
        {
            if(!activeBattlers[i].isPlayer)
            {
                Enemies.Add(i);
            }
        }

        for(int i=0; i<targetButtons.Length;i++)
        {
            if(Enemies.Count >i && activeBattlers[Enemies[i]].currentHP >0)
            {
                targetButtons[i].gameObject.SetActive(true);
                targetButtons[i].moveName = moveName;
                targetButtons[i].activeBattlerTarget = Enemies[i];
                targetButtons[i].targetName.text = activeBattlers[Enemies[i]].charName;

            }    
            else
            {
                targetButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenMagicMenu()
    {
        magicMenu.SetActive(true);

        
        for(int i=0; i<magicButtons.Length;i++)
        {
            if(activeBattlers[currentTurn].movesAvailable.Length> i)
            {
                magicButtons[i].gameObject.SetActive(true);
                
                magicButtons[i].spellName = activeBattlers[currentTurn].movesAvailable[i];
                magicButtons[i].nameText.text = magicButtons[i].spellName;
                
                
                for(int j=0; j<moveList.Length;j++)
                {
                    if(moveList[j].moveName == magicButtons[i].spellName)
                    {
                        magicButtons[i].spellCost = moveList[j].moveCost;
                        magicButtons[i].costText.text = magicButtons[i].spellCost.ToString();
                    }
                }
            }
            else
            {
                magicButtons[i].gameObject.SetActive(false);
            }
        }
        
    }

    public void flee()
    {
        int fleeSuccess = Random.Range(0, 100);
        if(fleeSuccess < chanceToFLee)
        {
            //end the battle
            //battleActive = false;
           // battleScene.SetActive(false);
            fleeing = true;
            StartCoroutine(EndBattleCo());
        }
        else
        {
            NextTurn();
            battleNotice.thetext.text = " Couldn't escape!";
            battleNotice.Activate();
        }
    }

    public void ShowItem()
    {
        itemMenu.SetActive(true);
        GameManager.instance.SortItems();
        for(int i = 0; i< itemButtons.Length;i++)
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

    public void SelectedItem(Item newItem)
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
        itemDescription.text = activeItem.description;
    }

    public void OpenItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(true);

        for(int i = 0; i<itemCharChoiceNames.Length;i++)
        {
            itemCharChoiceNames[i].text = GameManager.instance.playerStats[i].CharName;
            itemCharChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.instance.playerStats[i].gameObject.activeInHierarchy);        
        }
    }

    public void CloseItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(false);
    }

    public void UseItem(int selectedChar)
    {
        activeItem.Use(selectedChar);
        CloseItemCharChoice();
        itemMenu.SetActive(false);
        NextTurn();
    }

    public IEnumerator EndBattleCo()
    {
        battleActive = false;
        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);
        magicMenu.SetActive(false);
        itemMenu.SetActive(false);

        yield return new WaitForSeconds(.5f);

        UIFade.instance.FadeToBlack();

        yield return new WaitForSeconds(1.5f);

        for(int i=0; i<activeBattlers.Count; i++)
        {
            if(activeBattlers[i].isPlayer)
            {
                for(int j= 0; j< GameManager.instance.playerStats.Length;j++)
                {
                    if(activeBattlers[i].charName == GameManager.instance.playerStats[j].CharName)
                    {
                        GameManager.instance.playerStats[j].currentHP = activeBattlers[i].currentHP;
                        GameManager.instance.playerStats[j].currentMP = activeBattlers[i].currentMP;

                    }
                }
            }
            Destroy(activeBattlers[i].gameObject);
        }
        UIFade.instance.fadeFromBlack();
        battleScene.SetActive(false);
        activeBattlers.Clear();
        currentTurn = 0;
        //GameManager.instance.battleActive = false;
        if(fleeing)
        {
            GameManager.instance.battleActive = false;
            fleeing = false;
        }
        else
        {
            BattleReward.instance.OpenRewardScreen(rewardXP,rewardItems);
        }
        AudioManager.instance.PlayBGM(FindObjectOfType<CameraController>().musicToPlay);
        

    }

    public IEnumerator GameOverCo()
    {
        battleActive = false;
        UIFade.instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);
        battleScene.SetActive(false);

        SceneManager.LoadScene(gameOverScene);
    }
}
