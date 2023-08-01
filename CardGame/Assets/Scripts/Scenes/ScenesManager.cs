using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { START, BATTLE, REST, EVENT, BOSS, END}

public class ScenesManager : MonoBehaviour
{
    [SerializeField] GameObject gameView;
    [SerializeField] GameObject scenesPosition;
    [SerializeField] GameObject modal;

    [SerializeField] GameObject gameBarPrefab;
    [SerializeField] GameObject startScenePrefab;
    [SerializeField] GameObject battleScenePrefab;
    [SerializeField] private Character SelectedCharacter;


    private GameObject gameBar;
    private GameObject startScene;
    private GameObject battleScene;
    private ShowMap mapButton;
    public GameObject activeSCene;
    public BattleScene battleSceneComponent;
    public StartScene startSceneComponent;
    public GameBar gameBarComponent;
    public GameState gameState;
    public MapInfo mapData;

    public GameObject playerOnScene;
    public Player playerData;

    // Start is called before the first frame update
    void Start()
    {
        gameBar = Instantiate(gameBarPrefab, gameView.transform);
        gameBarComponent = gameBar.GetComponent<GameBar>();
        mapButton = gameBarComponent.mapButton.GetComponent<ShowMap>();
        mapData = mapButton.map.GetComponent<MapInfo>();
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
                    startSceneComponent = startScene.GetComponent<StartScene>();

                }

                if (activeSCene != null)
                {
                    activeSCene.SetActive(false);
                }

                if (playerOnScene == null)
                {
                    InstantiateNewPlayer();
                } 
                else
                {
                    playerData = playerOnScene.GetComponent<Player>();
                }

                playerOnScene.transform.SetParent(startSceneComponent.playerArea.transform, false);
                startSceneComponent.playerData = playerData;

                activeSCene = startScene;
                activeSCene.SetActive(true);
            break;
            case GameState.BATTLE:
                activeSCene.SetActive(false);

                if (battleScene == null)
                {
                    battleScene = Instantiate(battleScenePrefab, scenesPosition.transform);
                    battleSceneComponent = battleScene.GetComponent<BattleScene>();

                }

                playerOnScene.transform.SetParent(battleSceneComponent.playerArea.transform, false);

                activeSCene = battleScene;
                activeSCene.SetActive(true);
            break;
        }
    }

    public void InstantiateNewPlayer()
    {
        GameObject newPlayerOnScene = Instantiate(SelectedCharacter.characterPrefab);
        Character playerCopy = Instantiate(SelectedCharacter);

        newPlayerOnScene.name = "Player";
        playerData = newPlayerOnScene.GetComponent<Player>();
        playerData.data = playerCopy;
        playerData.data.updateBattleHUD = newPlayerOnScene.GetComponent<CharactersHUD>();
        playerData.pjArt.sprite = SelectedCharacter.splashArt;
        playerOnScene = newPlayerOnScene;
    }
}
