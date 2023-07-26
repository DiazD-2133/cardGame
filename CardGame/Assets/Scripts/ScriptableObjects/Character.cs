using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class Character : ScriptableObject
{
	public CharacterClass characterClass;
    public enum CharacterClass{Warrior, Assassin, Enemy, Boss}
    public GameObject characterPrefab;
    // public Relic startingRelic;
    public Sprite splashArt;
    public List<Card> startingDeck;
    public int maxHealth;
    public int currentHealth;
    public int maxMovements;
    public int currentMovements;
    public int drawValue;
    public int armor;
    public List<BuffsAndDebuffs> CharacterBuffsList = new List<BuffsAndDebuffs>();
    public List<BuffsAndDebuffs> CharacterDebuffsList = new List<BuffsAndDebuffs>();
    public CharactersHUD updateBattleHUD;

    public void GetCurrentMovements()
    {
        currentMovements = maxMovements;
        updateBattleHUD.UpdateCurrentMovements(currentMovements);
    }

    public void GainMovement(int movement)
    {
        currentMovements += movement;
        updateBattleHUD.UpdateCurrentMovements(currentMovements);
    }
    
    public void GainArmor(int newArmor)
    {
        armor += newArmor;
        updateBattleHUD.updateArmorHUD(armor);
    }
    public int SetArmor(int dmg)
    {
        if (dmg > armor)
        {
            dmg -= armor;
            armor = 0;
            updateBattleHUD.updateArmorHUD(armor);
            return dmg;
        }
        else
        {
            armor -= dmg;
            updateBattleHUD.updateArmorHUD(armor);
            return 0;
        }
    }

    public bool TakeDamage(int dmg) 
    {
        if(armor > 0) 
        {
            dmg = SetArmor(dmg);
        }
        
        if (dmg > 0)
        {
            currentHealth -= dmg;
            updateBattleHUD.UpdateHPBar(currentHealth, maxHealth);
        }
        
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
