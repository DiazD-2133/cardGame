using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public List<CharacterData> normalEnemies;
    public GameObject enemiesArea;
    public List<GameObject> enemiesOnScene;


    public void InstantiateEnemies(int totalEnemies, CharacterData enemy)
    {
        for (int i = 0; i < totalEnemies; i++)
        {
            GameObject newEnemy = Instantiate(enemy.characterPrefab, enemiesArea.transform);
            CharacterData enemyCopy = Instantiate(enemy);

            newEnemy.name = $"{enemy.name} {i}";
            CharacterElements enemyData = newEnemy.GetComponent<CharacterElements>();
            enemyData.data = enemyCopy;
            newEnemy.GetComponent<EnemyBehaviour>().enemyData = enemyData;
            enemyData.data.updateBattleHUD = newEnemy.GetComponent<CharactersHUD>();
            enemyData.characterImage.sprite = enemy.splashArt;

            if (enemyData.deck.Count == 0)
            {
                enemyData.createDeck(enemyData.deck);
            }

            enemiesOnScene.Add(newEnemy);
        }
    }
}
