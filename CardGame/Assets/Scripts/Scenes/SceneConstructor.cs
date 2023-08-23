using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneConstructor : MonoBehaviour
{
    public NodeInfo nodeData;
    private int totalEnemies;
    [SerializeField] private EnemiesManager enemies;
    [SerializeField] private BattleSystem battleSystem;
    [SerializeField] private ScenesManager scenesManager;

    

    public void NodeTypeSceneCreator()
    {
        switch (nodeData.roomType.roomName){
            case RoomType.NormalEnemy:
                scenesManager.gameState = GameState.BATTLE;
                scenesManager.ChangeScene();

                nodeData.FillData();
                totalEnemies = 2;
                enemies.enemiesArea = battleSystem.scenesManager.battleSceneComponent.enemiesArea;
                enemies.InstantiateEnemies(totalEnemies, nodeData.characterOnScene);
                battleSystem.state = BattleState.START;
                StartCoroutine(battleSystem.SetupBattle());
            break;
            case RoomType.Rest:
                Debug.Log("Rest");
                scenesManager.mapData.NextPositions(scenesManager.mapData.playerPosition);
            break;
            case RoomType.EliteEnemy:
                Debug.Log("EliteEnemy");
                scenesManager.mapData.NextPositions(scenesManager.mapData.playerPosition);
            break;
        }

    }
}
