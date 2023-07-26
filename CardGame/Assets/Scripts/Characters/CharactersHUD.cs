using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharactersHUD : MonoBehaviour
{
    [SerializeField] private Player characterData;
    [SerializeField] private TextMeshProUGUI maxHealthText;
    [SerializeField] private TextMeshProUGUI currentHealthText;
    [SerializeField] private TextMeshProUGUI currentArmorText;
    [SerializeField] private TextMeshProUGUI currentMovementsText;
    [SerializeField] private TextMeshProUGUI maxMovementsText;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private GameObject armorImage;
    private Color originalHealthBarColor;

    private Vector2 healthBarOriginalSize;

    // Start is called before the first frame update
    void Start()
    {
        originalHealthBarColor = healthBarImage.color;
        GenerateHPBar();
        UpdateHPBar(characterData.data.currentHealth, characterData.data.maxHealth);

        if (maxMovementsText != null && currentMovementsText != null)
        {
            GetMaxMovements();
            characterData.data.GetCurrentMovements();
        }

        if(characterData.data.armor > 0)
        {
            updateArmorHUD(characterData.data.armor);
        }
    }

    private void GenerateHPBar()
    {
        healthBarOriginalSize = healthBarImage.rectTransform.sizeDelta;
        maxHealthText.text = characterData.data.maxHealth.ToString();
    }
    private void GetMaxMovements()
    {
        maxMovementsText.text = characterData.data.maxMovements.ToString();
    }

    public void UpdateHPBar(int currentHP, int maxHP)
    {
        if (currentHP < 0)
        {
            currentHP = 0;
        }

        currentHealthText.text = currentHP.ToString();

        // Set healthBar width to a percentage of its original value
        // healthBarOriginalSize.x * (health/ maxHealth)
            
        healthBarImage.rectTransform.sizeDelta = new Vector2(healthBarOriginalSize.x * ((float)currentHP / (float)maxHP), healthBarImage.rectTransform.sizeDelta.y);
    }

    public void UpdateCurrentMovements(int currentMovements)
    {
        currentMovementsText.text = currentMovements.ToString();
    }

    public void updateArmorHUD(int armor)
    {
        if (armor > 0)
        {
            healthBarImage.color = Color.grey;
            currentArmorText.text = armor.ToString();
            armorImage.SetActive(true);
        }
        else
        {
            healthBarImage.color = originalHealthBarColor;
            armorImage.SetActive(false);
        }
    }
}

