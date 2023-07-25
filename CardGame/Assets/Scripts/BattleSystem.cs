using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Character;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    [SerializeField] private Character SelectedCharacter;
    [SerializeField] private CardListener enemiesActionListener;
    [SerializeField] private EnemiesManager enemiesManager;
    [SerializeField] private DecksAndDraw decksAndDraw;
    [SerializeField] private GameObject playerArea;

    private GameObject playerOnScene;
    private Player playerData;

    [SerializeField] private Button endPlayerTurnButton;

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    private void InstantiateNewPlayer()
    {
        GameObject newPlayerOnScene = Instantiate(SelectedCharacter.characterPrefab, playerArea.transform);
        Character playerCopy = Instantiate(SelectedCharacter);

        newPlayerOnScene.name = "Player";
        playerData = newPlayerOnScene.GetComponent<Player>();
        playerData.data = playerCopy;
        playerData.data.updateBattleHUD = newPlayerOnScene.GetComponent<BattleHUD>();
        playerData.pjArt.sprite = SelectedCharacter.splashArt;
        playerOnScene = newPlayerOnScene;
    }

    IEnumerator SetupBattle()
    {
        if (playerOnScene == null)
        {
            InstantiateNewPlayer();
        } 
        else
        {
            playerData = playerOnScene.GetComponent<Player>();
        }

        if (playerData.deck.Count == 0)
        {
            playerData.createDeck(playerData.deck);
        }

        decksAndDraw.InstantiatePlayerCards(playerData);

        enemiesManager.InstantiateEnemies();

        yield return new WaitForSeconds(1f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    private void GetEnemyActionInfo()
    {
        foreach (var enemy in enemiesManager.enemiesOnScene)
        {
            List<Card> enemyActions = enemy.GetComponent<EnemyBehaviour>().startingDeck;
            List<float> actionsProbabilities = enemy.GetComponent<EnemyBehaviour>().probabilities;
            
            Card selectedAction = enemy.GetComponent<EnemyBehaviour>().ChooseAction(enemyActions, actionsProbabilities);

            enemy.GetComponent<EnemyBehaviour>().selectedAction = selectedAction;

            print($"{enemy.name} uses: {selectedAction.description}");

        }
    }

    public void PlayerTurn()
    {
        if (!endPlayerTurnButton.interactable)
        {
            endPlayerTurnButton.interactable = true;
        }

        GetEnemyActionInfo();

        if (playerData.data.armor > 0)
        {
            playerData.data.armor = 0;
            playerData.data.SetArmor(playerData.data.armor);
        }
        
        decksAndDraw.DrawCards(playerData.data.drawValue);
    }

    public IEnumerator AttackEnemy(GameObject enemy, int dmg)
    {
        Player enemyData = enemy.GetComponent<Player>();
        bool isDead = enemyData.data.TakeDamage(dmg);

        if (isDead)
        {
            enemiesManager.enemiesOnScene.Remove(enemy);
            Destroy(enemy);
        }
        
        yield return new WaitForSeconds(0.1f);

        Debug.Log($"Enemies on scene = {enemiesManager.enemiesOnScene.Count}");


        if (enemiesManager.enemiesOnScene.Count == 0)
        {
            state = BattleState.WON;
            EndBattle();
        }

    }

    public void ApplyBuffDebuff(Card cardData, BuffsAndDebuffs status, GameObject enemy = null)
    {
        List<CardStatuses> BuffsList = new() { CardStatuses.Block, CardStatuses.Dexterity, CardStatuses.Strength, CardStatuses.Reflect };
        List<CardStatuses> Debuff = new() { CardStatuses.Exhaust, CardStatuses.Poison, CardStatuses.Vulnerable, CardStatuses.Weak };

        if (cardData.target == Target.Enemy)
        {
            if (enemy != null) {
                Player enemyData = enemy.GetComponent<Player>();
                AddToBuffDebuffList(status, enemyData);
                Debug.Log(enemyData.data.CharacterDebuffsList[0].Status);
            }
            
        }
        else if (cardData.target == Target.Self)
        {
            AddToBuffDebuffList(status, playerData);
        }
        if (cardData.target == Target.Multiple)
        {
            foreach (var enemyOnScene in enemiesManager.enemiesOnScene)
            {
                Player enemyData = enemyOnScene.GetComponent<Player>();
                AddToBuffDebuffList(status, enemyData);
            }
        }
    }

    public IEnumerator EnemyTurn()
    {
        decksAndDraw.MoveToDiscardDeck();

        foreach (var enemy in enemiesManager.enemiesOnScene)
        {
            Player enemyData = enemy.GetComponent<Player>();
            if(enemyData.data.armor > 0)
            {
                enemyData.data.armor = 0;
                enemyData.data.SetArmor(enemyData.data.armor);
            }

            Card selectedAction = enemy.GetComponent<EnemyBehaviour>().selectedAction;

            enemiesActionListener.CallCardApplications(selectedAction, enemyData, playerOnScene);
            yield return new WaitForSeconds(1f);

        }

        // if is dead
        //  state = BattleState.Lost
        //  EndBattle();
        // else
        //  state = BattleState.PLAYERTURN
        //  PlayerTurn
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    private void EndBattle()
    {
        if (state == BattleState.WON)
        {
            Debug.Log("You Win!");
        }
        else if(state == BattleState.LOST)
        {
            Debug.Log("Game Over");
        }
    }

    private void AddToBuffDebuffList(BuffsAndDebuffs status, Player target)
    {
        List<CardStatuses> BuffsList = new() { CardStatuses.Block, CardStatuses.Dexterity, CardStatuses.Strength, CardStatuses.Reflect };
        List<CardStatuses> Debuff = new() { CardStatuses.Exhaust, CardStatuses.Poison, CardStatuses.Vulnerable, CardStatuses.Weak };

        if (BuffsList.Contains(status.Status))
        {
            int index = target.data.CharacterBuffsList.IndexOf(status);
            if (index != -1)
            {
                target.data.CharacterBuffsList[index].Value += status.Value;
            }
            else
            {
                target.data.CharacterBuffsList.Add(status);
            }

        }
        else if (Debuff.Contains(status.Status))
        {
            int index = target.data.CharacterDebuffsList.IndexOf(status);
            if (index != -1)
            {
                target.data.CharacterDebuffsList[index].Value += status.Value;
            }
            else
            {
                target.data.CharacterDebuffsList.Add(status);
            }
        }
    }
}
