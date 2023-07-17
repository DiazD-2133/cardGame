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
            enemyData.pjArt.sprite = enemiesList[i].splashArt;
            enemiesOnScene.Add(newEnemy);
        }
    }
}
