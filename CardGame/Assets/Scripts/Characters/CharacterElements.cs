using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterElements : MonoBehaviour
{
    public Image characterImage;
    public GameObject playerHUD;

    public CharacterData data;
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
