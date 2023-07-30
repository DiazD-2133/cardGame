using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    public BattleSystem battleSystem;

    public void EndPlayerTurn()
    {
        battleSystem.state = BattleState.ENEMYTURN;
        
        StartCoroutine(battleSystem.EnemyTurn());
    }
}
