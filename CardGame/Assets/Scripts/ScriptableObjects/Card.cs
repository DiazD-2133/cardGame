using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Level
{
    Initial,
    Common,
    Rare,
    Epic
}
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
    Break,
    Damage,
    Draw,
    GainMana,
    SelfDamage,
    Shield,
    Times,
}

public enum CardStatuses
{
    Block,
    Decrease,
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
    public CardActions Effect;
    public int Value;
    public int ImprovedValue;
}

[System.Serializable]
public class StatusEffect
{
    public CardStatuses Status;
    public int Value;
    public int ImprovedValue;
}

[CreateAssetMenu( fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public Sprite artwork;
    public Level cardLevel;

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

    [Header("Only Enemies Cards")]
    public float probability;


    // For Description
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
            int effectValue = IsImproved(effect.Value, effect.ImprovedValue);
            description += $"{descriptionSeparators[index]}{effect.Effect} {effectValue}";
                    index++;
        }
    }

    public void AssignStatuses(List<StatusEffect> effectsList)
    {
        foreach (var effect in effectsList)
        {
            int effectValue = IsImproved(effect.Value, effect.ImprovedValue);
            description += $"{descriptionSeparators[index]}{effect.Status} {effectValue}";
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
