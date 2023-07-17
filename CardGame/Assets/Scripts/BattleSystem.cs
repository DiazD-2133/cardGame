using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    public Character character;
    public EnemiesManager enemiesManager;
    public DecksAndDraw decksAndDraw;
    public GameObject playerArea;

    private GameObject playerOnScene;
    private Player playerData;
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
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

    public void PlayerTurn()
    {
        playerData.data.armor = 0;
        decksAndDraw.DrawCards(playerData.data.stats.drawValue);
    }

    public IEnumerator AttackEnemy(GameObject enemy, int dmg)
    {
        Player enemyData = enemy.GetComponent<Player>();
        bool isDead = enemyData.data.TakeDamage(dmg);

        enemy.GetComponent<BattleHUD>().UpdateCharacterHUD(enemyData);
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

    public IEnumerator EnemyTurn()
    {
        decksAndDraw.MoveToDiscardDeck();
        yield return new WaitForSeconds(1f);

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

    private void InstantiateNewPlayer()
    {
        GameObject newPlayerOnScene = Instantiate(character.characterPrefab, playerArea.transform);
        Character playerCopy = Instantiate(character);

        newPlayerOnScene.name = "Player";
        playerData = newPlayerOnScene.GetComponent<Player>();
        playerData.data = playerCopy;
        playerData.pjArt.sprite = character.splashArt;
        playerOnScene = newPlayerOnScene;
    }
}
