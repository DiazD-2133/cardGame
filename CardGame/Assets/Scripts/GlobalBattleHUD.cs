using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GlobalBattleHUD : MonoBehaviour
{
    public Player playerData;
    public EnemiesManager enemies;

    private GameObject player;
    public TextMeshProUGUI maxHealthText;
    public TextMeshProUGUI currentHealthText;
    public Image healthBarImage;

    private Vector2 healthBarOriginalSize;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerData = player.GetComponent<Player>();

        GetCharacterHealthUI(player);

        GenerateCharacterHUD(maxHealthText, healthBarImage, playerData);

        UpdateCharacterHUD(currentHealthText, healthBarImage, playerData);

        foreach(var enemy in enemies.enemiesOnScene)
        {
            Player enemyData = enemy.GetComponent<Player>();
            Transform statsTransform = enemy.transform.Find("Stats");

            GetCharacterHealthUI(enemy);

            if (healthBarImage != null && currentHealthText != null && maxHealthText != null)
            {
                GenerateCharacterHUD(maxHealthText, healthBarImage, enemyData);
                UpdateCharacterHUD(currentHealthText, healthBarImage, enemyData);
            }
                
        }
    }

    private void GenerateCharacterHUD(TextMeshProUGUI characterMaxHealth, Image characterHealthBar, Player characterData)
    {
        healthBarOriginalSize = characterHealthBar.rectTransform.sizeDelta;
        characterMaxHealth.text = characterData.data.maxHealth.ToString();
    }

    public void UpdateCharacterHUD(TextMeshProUGUI currentHealth, Image healthBar, Player character)
    {
        if (character != null)
        {
            if (character.data.currentHealth < 0)
            {
                character.data.currentHealth = 0;
            }

            currentHealth.text = character.data.currentHealth.ToString();

            // Set healthBar width to a percentage of its original value
            // healthBarOriginalSize.x * (health/ maxHealth)
            
            healthBar.rectTransform.sizeDelta = new Vector2(healthBarOriginalSize.x * ((float)character.data.currentHealth/ (float)character.data.maxHealth), healthBar.rectTransform.sizeDelta.y);
        }
    }

    public void UpdateEnemyHUD(GameObject enemy)
    {
        Player enemyData = enemy.GetComponent<Player>();

        GetCharacterHealthUI(enemy);

        if (healthBarImage != null && currentHealthText != null && maxHealthText != null)
        {
            UpdateCharacterHUD(currentHealthText, healthBarImage, enemyData);
        }
            
    }

    public void GetCharacterHealthUI(GameObject Character)
    {
        Transform statsTransform = Character.transform.Find("Stats");

            if (statsTransform != null)
            {
                Transform healthBarTransform = statsTransform.Find("Health Bar");
                if (healthBarTransform != null)
                {
                    healthBarImage = healthBarTransform.Find("Current Health Bar Status").GetComponent<Image>();
                    currentHealthText = healthBarTransform.Find("Current Health").GetComponent<TextMeshProUGUI>();
                    maxHealthText = healthBarTransform.Find("Max Health").GetComponent<TextMeshProUGUI>();
                }
            }    
    }

}

