using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseMap : MonoBehaviour
{
    [SerializeField] private MapInfo map;
    [SerializeField] private GenerateRandomMap generateMap;
    [SerializeField] private List<EnemiesList> mapEnemies = new List<EnemiesList>();

    public void GeneraRandomMap()
    {
        map.stageName = "Floor One";
        map.selectedMap = mapEnemies[0];
        generateMap.ClearMap();
        generateMap.GenerateMap();
        map.InitializePlayerPosition();
    }
}
