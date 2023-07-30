using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public List<Character> normalEnemies;
    public GameObject enemiesArea;
    public List<GameObject> enemiesOnScene;


    public void InstantiateEnemies(int totalEnemies, Character enemy)
    {
        for (int i = 0; i < totalEnemies; i++)
        {
            GameObject newEnemy = Instantiate(enemy.characterPrefab, enemiesArea.transform);
            Character enemyCopy = Instantiate(enemy);

            newEnemy.name = $"{enemy.name} {i}";
            Player enemyData = newEnemy.GetComponent<Player>();
            enemyData.data = enemyCopy;
            newEnemy.GetComponent<EnemyBehaviour>().enemyData = enemyData;
            enemyData.data.updateBattleHUD = newEnemy.GetComponent<CharactersHUD>();
            enemyData.pjArt.sprite = enemy.splashArt;

            if (enemyData.deck.Count == 0)
            {
                enemyData.createDeck(enemyData.deck);
            }

            enemiesOnScene.Add(newEnemy);
        }
    }
}
