using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public List<Character> enemiesList;
    public GameObject enemiesArea;
    public List<GameObject> enemiesOnScene;


    public void InstantiateEnemies()
    {
        for (int i = 0; i < enemiesList.Count; i++)
        {
            GameObject newEnemy = Instantiate(enemiesList[i].characterPrefab, enemiesArea.transform);
            Character enemyCopy = Instantiate(enemiesList[i]);

            newEnemy.name = $"{enemiesList[i].name} {i}";
            Player enemyData = newEnemy.GetComponent<Player>();
            enemyData.data = enemyCopy;
            newEnemy.GetComponent<EnemyBehaviour>().enemyData = enemyData;
            enemyData.data.updateBattleHUD = newEnemy.GetComponent<CharactersHUD>();
            enemyData.pjArt.sprite = enemiesList[i].splashArt;

            if (enemyData.deck.Count == 0)
            {
                enemyData.createDeck(enemyData.deck);
            }

            enemiesOnScene.Add(newEnemy);
        }
    }
}
