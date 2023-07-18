using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Character;

public class CardListener : MonoBehaviour
{
    [SerializeField] private BattleSystem battleSystem;

    private void ApplyActions(CardActions action, int value, Player playerData = null, GameObject enemy = null)
    {
        switch (action) {
            case CardActions.Shield:
                if (playerData != null)
                {
                    playerData.data.GainArmor(value);
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
            case CardActions.SelfDamage:
                break;
            case CardActions.Times:
                break;
        }
    }
    
    private void ApplyStatuses(Card cardData, CardStatuses status, int value, Player playerData = null, GameObject enemy = null)
    {
        switch (status) {
            case CardStatuses.Block:
                break;
            case CardStatuses.Dexterity:
                break;
            case CardStatuses.Exhaust:
                break;
            case CardStatuses.Poison:
                break;
            case CardStatuses.Reflect:
                break;
            case CardStatuses.Strength:
                break;
            case CardStatuses.Vulnerable:
                BuffsAndDebuffs myBuff = new BuffsAndDebuffs(status, value);
                battleSystem.ApplyBuffDebuff(cardData, myBuff, enemy);

                break;
            case CardStatuses.Weak:
                break;
        }
    }

    private void GetActions(Card cardData, Player playerData = null, GameObject enemy = null)
    {
        foreach (var action in cardData.actionsList)
        {
            int effectValue = cardData.IsImproved(action.Value, action.ImprovedValue);
            Debug.Log($"{action.Effect} = {effectValue}");
            ApplyActions(action.Effect, effectValue, playerData, enemy);
        }
    }

    private void ApplyStatuses(Card cardData, Player playerData = null, GameObject enemy = null)
    {
        foreach(var status in cardData.statusesList)
        {
            int effectValue = cardData.IsImproved(status.Value, status.ImprovedValue);
            Debug.Log($"{status.Status} = {effectValue}");
            ApplyStatuses(cardData, status.Status, effectValue, playerData, enemy);
        }
    }

    public void CallApplications(Card cardData, Player playerData, GameObject enemy = null)
    {
        if (enemy != null && playerData != null)
        {
            GetActions(cardData, playerData, enemy);
            ApplyStatuses(cardData, playerData, enemy);
            return;
        }
        
        GetActions(cardData, playerData);
        ApplyStatuses(cardData);
    }

}
