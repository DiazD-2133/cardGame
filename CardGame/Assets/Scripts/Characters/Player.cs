using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Image pjArt;

    public Character data;
    public GameObject playerHUD;


    public List<Card> deck;


    public void createDeck(List<Card> playerDeck)
    {
        foreach (Card card in data.startingDeck)
        {
            Card cardInstance = Instantiate(card);
            playerDeck.Add(cardInstance);
        }
    }

}
