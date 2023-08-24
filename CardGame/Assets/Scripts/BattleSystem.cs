using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CharacterData;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    public ScenesManager scenesManager;
    public Button endPlayerTurnButton;


    [SerializeField] private CharacterData SelectedCharacter;
    [SerializeField] private CardListener enemiesActionListener;
    [SerializeField] private EnemiesManager enemiesManager;
    [SerializeField] private DecksAndDraw decksAndDraw;
    [SerializeField] private GameObject playerArea;

    private GameObject playerOnScene;
    private CharacterElements playerData;

    private void InstantiateNewPlayer()
    {
        playerArea = scenesManager.battleSceneComponent.playerArea;
        GameObject newPlayerOnScene = Instantiate(SelectedCharacter.characterPrefab, playerArea.transform);
        CharacterData playerCopy = Instantiate(SelectedCharacter);

        newPlayerOnScene.name = "Player";
        playerData = newPlayerOnScene.GetComponent<CharacterElements>();
        playerData.data = playerCopy;
        playerData.data.updateBattleHUD = newPlayerOnScene.GetComponent<CharactersHUD>();
        playerData.characterImage.sprite = SelectedCharacter.splashArt;
        playerOnScene = newPlayerOnScene;
    }

    public IEnumerator SetupBattle()
    {
        if (playerOnScene == null)
        {
            playerData = scenesManager.playerData;
            playerOnScene = scenesManager.playerOnScene;
        }

        if (playerData.deck.Count == 0)
        {
            playerData.createDeck(playerData.deck);
        }

        playerData.playerHUD.SetActive(true);

        // Positioning the player
        playerArea = scenesManager.battleSceneComponent.playerArea;
        playerOnScene.transform.SetParent(playerArea.transform, false);

        // Setup player cards
        decksAndDraw.playerHand = scenesManager.battleSceneComponent.playerHand;
        decksAndDraw.InstantiatePlayerCards(playerData);

        endPlayerTurnButton = scenesManager.battleSceneComponent.endPlayerTurnButton;
        endPlayerTurnButton.GetComponent<EndTurnButton>().battleSystem = this;

        yield return new WaitForSeconds(1f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    private void GetEnemyActionInfo()
    {
        if(enemiesManager.enemiesOnScene.Count != 0){
        foreach (var enemy in enemiesManager.enemiesOnScene)
            {
                List<Card> enemyActions = enemy.GetComponent<EnemyBehaviour>().startingDeck;
                List<float> actionsProbabilities = enemy.GetComponent<EnemyBehaviour>().probabilities;
                
                Card selectedAction = enemy.GetComponent<EnemyBehaviour>().ChooseAction(enemyActions, actionsProbabilities);

                enemy.GetComponent<EnemyBehaviour>().selectedAction = selectedAction;

                print($"{enemy.name} uses: {selectedAction.description}");

            }
        }
        else
        {
            Debug.Log("No Enemies on Scene");
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
        CharacterElements enemyData = enemy.GetComponent<CharacterElements>();
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
                CharacterElements enemyData = enemy.GetComponent<CharacterElements>();
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
                CharacterElements enemyData = enemyOnScene.GetComponent<CharacterElements>();
                AddToBuffDebuffList(status, enemyData);
            }
        }
    }

    public IEnumerator EnemyTurn()
    {
        if (endPlayerTurnButton.interactable)
        {
            endPlayerTurnButton.interactable = false;
        }

        decksAndDraw.MoveToDiscardDeck();

        foreach (var enemy in enemiesManager.enemiesOnScene)
        {
            CharacterElements enemyData = enemy.GetComponent<CharacterElements>();
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
            playerData.deck.Clear();
            Debug.Log("You Win!");

            playerData.playerHUD.SetActive(false);

            decksAndDraw.MoveToDiscardDeck();
            decksAndDraw.MoveToDeck();
            scenesManager.mapData.NextPositions(scenesManager.mapData.playerPosition);

        }
        else if(state == BattleState.LOST)
        {
            Debug.Log("Game Over");
        }

        
    }

    private void AddToBuffDebuffList(BuffsAndDebuffs status, CharacterElements target)
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
