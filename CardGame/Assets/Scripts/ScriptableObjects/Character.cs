using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class Character : ScriptableObject
{
	public CharacterClass characterClass;
    public enum CharacterClass{Warrior,Assassin,Enemy}
    public GameObject characterPrefab;
    // public Relic startingRelic;
    public Sprite splashArt;
    public List<Card> startingDeck;
    public int maxHealth;
    public int currentHealth;
    public int maxMana;
    public int currentMana;
    public int drawValue;
    public int armor;
    public List<BuffsAndDebuffs> CharacterBuffsList = new List<BuffsAndDebuffs>();
    public List<BuffsAndDebuffs> CharacterDebuffsList = new List<BuffsAndDebuffs>();

    public bool TakeDamage(int dmg) 
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public class BuffsAndDebuffs
    {
        public CardStatuses Status { get; set; }
        public int Value { get; set; }

        public BuffsAndDebuffs(CardStatuses status, int value)
        {
            Status = status;
            Value = value;
        }
    }

}
