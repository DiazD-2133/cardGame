using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CharacterData;

[CreateAssetMenu( fileName = "New Card list", menuName = "Class cards list")]
public class ClassCardRewards : ScriptableObject
{
    [SerializeField] CharacterClass characterClass;
    public List<Card> commonCards;
    public List<Card> rareCards;
    public List<Card> epicsCards;
    public List<Card> unbailableCards;


}
