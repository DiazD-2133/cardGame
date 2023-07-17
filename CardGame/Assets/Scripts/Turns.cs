using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turns : MonoBehaviour
{
    [SerializeField] private DecksAndDraw decksAndDrawManager;
    [SerializeField] private BattleSystem battleSystem;
    [SerializeField] private Button startTurnButton;
    [SerializeField] private Button endPlayerTurnButton;

    public void EndPlayerTurn()
    {
        battleSystem.state = BattleState.ENEMYTURN;
        
        endPlayerTurnButton.interactable = false;
        startTurnButton.interactable = true;

        StartCoroutine(battleSystem.EnemyTurn());
    }

    public void StartTurn()
    {
        startTurnButton.interactable = false;
        endPlayerTurnButton.interactable = true;
    }
    
    public void DrawOne()
    {
        decksAndDrawManager.DrawCards(1);
    }
}
