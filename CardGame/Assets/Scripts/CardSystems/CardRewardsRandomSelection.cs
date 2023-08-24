using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRewardsRandomSelection : MonoBehaviour
{
    [SerializeField] CardLevelRandomSelection cardLevelSelector;
    [SerializeField] ClassCardRewards cardsLists;
    public Card selectedCard;


    void Start()
    {
        ChooseACard();
        Debug.Log(selectedCard.cardName);
    }

    private void ChooseACard()
    {
        Level cardLevel = cardLevelSelector.ChooseCardLevel();
        int index;
        Debug.Log(cardLevel);
        switch (cardLevel)
        {
            case Level.Common:
                List<Card> commonCards = cardsLists.commonCards;
                index = Random.Range(0, commonCards.Count);
                selectedCard = commonCards[index];
                IncreaseEpicCardProbability();
                
                break;
            case Level.Rare:
                List<Card> rareCards = cardsLists.rareCards;
                index = Random.Range(0, rareCards.Count);
                selectedCard = rareCards[index];
                IncreaseEpicCardProbability();

                break;

            case Level.Epic:
                List<Card> epicCards = cardsLists.epicsCards;
                index = Random.Range(0, epicCards.Count);
                selectedCard = epicCards[index];
                cardLevelSelector.epicChanceBoost = -0.05f;
                break;
        }
    }

    private void IncreaseEpicCardProbability()
    {
        if (cardLevelSelector.epicChanceBoost < 0.41f)
        {
            cardLevelSelector.epicChanceBoost += 0.01f;
        }
    }
}
