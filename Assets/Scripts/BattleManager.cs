using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
            }
            else
            {
                if(activeBattlers[i].isPlayer)
                {
                    allPlayersDead = false;

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
                //end battle i victory
            }
            else
            {
                //end battle in failure
            }

            battleScene.SetActive(false);
            GameManager.instance.battleActive = false;
            battleActive = false;
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

        float damagecalc = (attackPower/defencePower) * Random.Range(.9f,1.1f);
        int damageToGive = Mathf.RoundToInt(damagecalc);

        Debug.Log(activeBattlers[currentTurn].charName + " is dealing" +damagecalc + "(" + damageToGive + ") damage to " + activeBattlers[target].charName);

        activeBattlers[target].currentHP -= damageToGive;

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
            if(Enemies.Count >i)
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
}
