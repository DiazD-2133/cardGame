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
    public int armor;
    public int maxMana;
    public int currentMana;

    public PlayerDataContainer stats;

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
}
