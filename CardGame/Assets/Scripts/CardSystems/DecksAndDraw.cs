using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecksAndDraw : MonoBehaviour
{
    public GameObject cardPrefab; // Card Prefab
    public GameObject playerDeck; // Player Deck(Child of Player prefab)
    public GameObject discardDeck;  // Discard Deck(Child of Game Manager)
    public GameObject playerHand; // Cards Position GameObject
    public CharacterElements playerData;

    // For Cards movement
    List <GameObject> cards = new List<GameObject>();
    List <GameObject> cardsOnPlayerArea = new List<GameObject>();
    List <GameObject> discardCards = new List<GameObject>();

    public void DrawCards(int drawValue)
    {
        if ((cardsOnPlayerArea.Count + drawValue) <= 8)
        {
            int randomIndex;

            while (drawValue > 0)
            {
                if (cards.Count == 0)
                {
                    MoveToDeck();
                }

                randomIndex = Random.Range(0, cards.Count);

                GameObject cardToMove = cards[randomIndex];
                cardToMove.transform.SetParent(playerHand.transform, false);
                cardToMove.SetActive(true);

                cardsOnPlayerArea.Add(cardToMove);
                cards.RemoveAt(randomIndex);

                drawValue--;
            }
        }
    }

    public void MoveToDiscardDeck()
    {
        foreach (var card in cardsOnPlayerArea)
        {
            discardCards.Add(card);
            card.transform.SetParent(discardDeck.transform, false);
            card.SetActive(false);
        }

        cardsOnPlayerArea.Clear();
    }

    public void MoveOneToDiscardDeck(GameObject PlayedCard)
    {
        discardCards.Add(PlayedCard);
        PlayedCard.transform.SetParent(discardDeck.transform, false);
        PlayedCard.SetActive(false);

        if (cardsOnPlayerArea.Contains(PlayedCard))
        {
            cardsOnPlayerArea.Remove(PlayedCard);
        }
    }

    public void InstantiatePlayerCards(CharacterElements playerData)
    {
        for (int i = 0; i < playerData.deck.Count; i++)
        {
            GameObject playerCard = Instantiate(cardPrefab, playerDeck.transform);
            playerCard.name = playerData.deck[i].cardName;
            playerCard.GetComponent<CardHUD>().card = playerData.deck[i];
            playerCard.GetComponent<CardHUD>().AssignCardData();
            cards.Add(playerCard);
            playerCard.SetActive(false);
        }
    }

    public void MoveToDeck()
    {
        foreach (var card in discardCards)
        {
            cards.Add(card);
            card.transform.SetParent(playerDeck.transform, false);
        }

        discardCards.Clear();
    }
}

