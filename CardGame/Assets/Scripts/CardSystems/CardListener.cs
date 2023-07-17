using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardListener : MonoBehaviour
{
    [SerializeField] private BattleSystem battleSystem;
    private void GetActions(Card cardData, Player playerData = null, GameObject enemy = null)
    {
        foreach(var action in cardData.actionsList)
        {
            int effectValue = cardData.IsImproved(action.value, action.improvedValue);
            Debug.Log($"{action.effect} = {effectValue}");
            ApplyActions(action.effect, effectValue, playerData, enemy);
        }
    }

    private void ApplyActions(CardActions action, int value, Player playerData = null, GameObject enemy = null)
    {
        switch (action) {
            case CardActions.Armor:
                if (playerData != null)
                {
                    playerData.data.armor += value;
                    Debug.Log("Actual armor = " + playerData.data.armor);
                }
                break;
            case CardActions.Damage:

                if (enemy != null)
                {
                    StartCoroutine(battleSystem.AttackEnemy(enemy, value));
                }
                break;
            case CardActions.Draw:
                break;
            case CardActions.GainMana:
                break;
            case CardActions.None:
                break;
            case CardActions.SelfDamage:
                break;
            case CardActions.Times:
                break;
        }
    }
    
    private void ApplyStatuses(Card cardData)
    {
        foreach(var status in cardData.statusesList)
        {
            int effectValue = cardData.IsImproved(status.value, status.improvedValue);
            Debug.Log($"{status.status} = {effectValue}");
        }
    }

    public void CallApplications(Card cardData, Player playerData, GameObject enemy = null)
    {
        if (enemy != null && playerData != null)
        {
            GetActions(cardData, playerData, enemy);
            ApplyStatuses(cardData);
            return;
        }
        
        GetActions(cardData, playerData);
        ApplyStatuses(cardData);
    }

}
