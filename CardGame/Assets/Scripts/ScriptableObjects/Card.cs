using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Ability,
    Attack,
    Power
}
public enum Target
{
    Self,
    Enemy,
    Multiple,
}

public enum CardActions
{
    Armor,
    Damage,
    Draw,
    GainMana,
    SelfDamage,
    Times,
}

public enum CardStatuses
{
    Block,
    Dexterity,
    Exhaust,
    Poison,
    Reflect,
    Strength,
    Vulnerable,
    Weak
}

[System.Serializable]
public class Action
{
    public CardActions effect;
    public int value;
    public int improvedValue;
}

[System.Serializable]
public class StatusEffect
{
    public CardStatuses status;
    public int value;
    public int improvedValue;
}

[CreateAssetMenu( fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public Sprite artwork;

    public string cardName;

    public List<Action> actionsList = new List<Action>();
    public List<StatusEffect> statusesList = new List<StatusEffect>();
    
    public CardType cardType;

    public bool isImproved;

    public int manaCost;
    public int improvedManaCost;
    public string description;

    public Target target;

    public bool selectable;
    private List<string> descriptionSeparators = new List<string>{"", " , ", " and "};
    private int index = 0;

    public void CreateCard()
    {
        description = "";
        switch (cardType) {
                case CardType.Ability:
                    if (actionsList.Count != 0){
                        AssignActions(actionsList);
                        if (description.Length > 0)
                        {
                            index = 1;
                        }
                    }
                    if (statusesList.Count != 0){
                        AssignStatuses(statusesList);
                    }
                    index = 0;
                    break;
                case CardType.Attack:
                    AssignActions(actionsList);
                    if (description.Length > 0)
                    {
                        index = 1;
                    }
                    if (statusesList.Count != 0){
                        AssignStatuses(statusesList);
                    }
                    index = 0;
                    break;
                case CardType.Power:
                    AssignStatuses(statusesList);
                    index = 0;
                    break;
        }
    }

    public void AssignActions(List<Action> actionsList)
    {
        foreach (var effect in actionsList)
        {
            int effectValue = IsImproved(effect.value, effect.improvedValue);
            description += $"{descriptionSeparators[index]}{effect.effect} {effectValue}";
                    index++;
        }
    }

    public void AssignStatuses(List<StatusEffect> effectsList)
    {
        foreach (var effect in effectsList)
        {
            int effectValue = IsImproved(effect.value, effect.improvedValue);
            description += $"{descriptionSeparators[index]}{effect.status} {effectValue}";
                    index++;
        }
    }

    public int IsImproved(int value, int improvedValue)
    {
        if (!isImproved) {
            return value;
        } else {
            return improvedValue;
        }
    }

}
