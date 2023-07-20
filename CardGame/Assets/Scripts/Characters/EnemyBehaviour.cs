using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public Player enemyData;
    public List<Card> startingDeck;
    public Card selectedAction;
    public List<string> actions = new (){"Damage 10", "weak 2"};
    public List<float> probabilities;

    void Start()
    {
        startingDeck = enemyData.data.startingDeck;
        foreach (var card in startingDeck)
        {
            card.CreateCard();
            probabilities.Add(card.probability);
        }
    }

    // Update is called once per frame
    public Card ChooseAction(List<Card> actions, List<float> probabilities)
    {
        float totalProbability = 0f;

        // Calcular la suma total de las probabilidades
        foreach (var probability in probabilities)
        {
            totalProbability += probability;
        }

        // Generar un número aleatorio entre 0 y la suma total de las probabilidades
        float randomValue = Random.Range(0f, totalProbability);

        // Realizar una iteración para determinar qué acción se selecciona
        for (int i = 0; i < actions.Count; i++)
        {
            if (randomValue < probabilities[i])
            {
                return actions[i];
            }

            randomValue -= probabilities[i];
        }

        // Si no se selecciona ninguna acción, devolver una acción predeterminada o null
        return null;
    }
}
