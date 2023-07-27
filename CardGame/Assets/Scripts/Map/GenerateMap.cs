using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    [SerializeField] GenerateRandomMap generateMap;
    // Start is called before the first frame update
    public void GeneraRandomMap()
    {
        generateMap.ClearMap();
        generateMap.GenerateMap();
    }
}
