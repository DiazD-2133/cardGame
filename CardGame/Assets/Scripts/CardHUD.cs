using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardHUD : MonoBehaviour
{
    
    public Card card;
    public TMP_Text cardName;
    public TMP_Text cardDescription;

    // Para despues
    public bool isImproved;

    public void AssignCardData()
    {
        card.CreateCard();
        cardName.text = card.cardName;
        cardDescription.text = card.description;
    }
}
