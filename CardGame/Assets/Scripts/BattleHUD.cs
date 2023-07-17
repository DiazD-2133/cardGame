using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    public Player playerData;
    public TextMeshProUGUI maxHealthText;
    public TextMeshProUGUI currentHealthText;
    public Image healthBarImage;

    private Vector2 healthBarOriginalSize;

    // Start is called before the first frame update
    void Start()
    {
        GenerateCharacterHUD(playerData);
        UpdateCharacterHUD(playerData);
    }

    private void GenerateCharacterHUD(Player characterData)
    {
        healthBarOriginalSize = healthBarImage.rectTransform.sizeDelta;
        maxHealthText.text = characterData.data.maxHealth.ToString();
    }

    public void UpdateCharacterHUD(Player character)
    {
        if (character != null)
        {
            if (character.data.currentHealth < 0)
            {
                character.data.currentHealth = 0;
            }

            currentHealthText.text = character.data.currentHealth.ToString();

            // Set healthBar width to a percentage of its original value
            // healthBarOriginalSize.x * (health/ maxHealth)
            
            healthBarImage.rectTransform.sizeDelta = new Vector2(healthBarOriginalSize.x * ((float)character.data.currentHealth/ (float)character.data.maxHealth), healthBarImage.rectTransform.sizeDelta.y);
        }
    }

    public void UpdateEnemyHUD(GameObject enemy)
    {
        Player enemyData = enemy.GetComponent<Player>();

        if (healthBarImage != null && currentHealthText != null && maxHealthText != null)
        {
            UpdateCharacterHUD(enemyData);
        }
            
    }
}

