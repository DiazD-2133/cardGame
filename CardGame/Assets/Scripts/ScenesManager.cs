using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { START, BATTLE, REST, EVENT, BOSS, END}

public class ScenesManager : MonoBehaviour
{
    [SerializeField] GameObject gameView;
    [SerializeField] GameObject modal;
    [SerializeField] GameObject scenesPosition;
    [SerializeField] GameObject gameBarPrefab;
    [SerializeField] GameObject startScenePrefab;
    [SerializeField] GameObject battleScenePrefab;

    private GameObject gameBar;
    private GameObject startScene;
    private GameObject battleScene;
    private ShowMap mapButton;
    public GameObject activeSCene;
    public BattleScene battleSceneComponent;
    public GameBar gameBarComponent;
    public GameState gameState;

    // Start is called before the first frame update
    void Start()
    {
        gameBar = Instantiate(gameBarPrefab, gameView.transform);
        gameBarComponent = gameBar.GetComponent<GameBar>();
        mapButton = gameBarComponent.mapButton.GetComponent<ShowMap>();
        mapButton.modal = modal;

        gameState = GameState.START;
        ChangeScene();
    }

    // Update is called once per frame
    public void ChangeScene()
    {
        switch (gameState)
        {
            case GameState.START:
                if (startScene == null)
                {
                    startScene = Instantiate(startScenePrefab, scenesPosition.transform);
                }

                if (activeSCene != null)
                {
                    activeSCene.SetActive(false);
                }

                activeSCene = startScene;
                activeSCene.SetActive(true);
            break;
            case GameState.BATTLE:
                activeSCene.SetActive(false);

                if (battleScene == null)
                {
                    battleScene = Instantiate(battleScenePrefab, scenesPosition.transform);
                }

                battleSceneComponent = battleScene.GetComponent<BattleScene>();
                activeSCene = battleScene;
                activeSCene.SetActive(true);
            break;
        }
    }
}
