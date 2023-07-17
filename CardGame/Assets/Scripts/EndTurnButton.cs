using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    [SerializeField] private BattleSystem battleSystem;
    [SerializeField] private Button endPlayerTurnButton;

    public void EndPlayerTurn()
    {
        battleSystem.state = BattleState.ENEMYTURN;
        
        endPlayerTurnButton.interactable = false;

        StartCoroutine(battleSystem.EnemyTurn());
    }
}
