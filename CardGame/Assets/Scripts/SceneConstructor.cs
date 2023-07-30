using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneConstructor : MonoBehaviour
{
    public NodeInfo nodeData;
    private int totalEnemies;
    [SerializeField] private EnemiesManager enemies;
    [SerializeField] private BattleSystem battleSystem;
    [SerializeField] private ScenesManager sceneManager;

    

    public void NodeTypeSceneCreator()
    {
        switch (nodeData.roomType.Name){
            case RoomType.NormalEnemy:
            sceneManager.gameState = GameState.BATTLE;
            sceneManager.ChangeScene();

            nodeData.FillData();
            totalEnemies = 2;
            enemies.enemiesArea = battleSystem.scenesManager.battleSceneComponent.enemiesArea;
            enemies.InstantiateEnemies(totalEnemies, nodeData.characterOnScene);
            battleSystem.state = BattleState.START;
            StartCoroutine(battleSystem.SetupBattle());
            break;
        }
    }
}
