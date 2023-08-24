using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CardLevelRandomSelection : MonoBehaviour
{
    private List<Level> levels = new List<Level> { Level.Common, Level.Rare, Level.Epic };
    private List<float> probabilities = new List<float> { 0.6f, 0.37f, 0.03f };
    public float epicChanceBoost = -0.05f;

    public Level ChooseCardLevel()
    {
        float totalProbability = probabilities.Sum();

        float randomValue = Random.Range(0f, totalProbability) + epicChanceBoost;

        for (int i = 0; i < levels.Count; i++)
        {
            if (randomValue < probabilities[i])
            {
                return levels[i];
            }

            randomValue -= probabilities[i];
        }

        return Level.Initial;
    }
}

