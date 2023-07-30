using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeInfo : MonoBehaviour
{
    public List<Rooms> roomTypesList = new List<Rooms>();
    [SerializeField] private Rooms normalEnemy;
    [SerializeField] private Rooms rest;
    [SerializeField] private Rooms boss;
    private MapInfo map;
    public Character characterOnScene;
    public Rooms roomType;
    private Image icon;

    [SerializeField] private Map.NodeMapInfo node;
    // Start is called before the first frame update
    void Start()
    {
        map = GameObject.Find("Map").GetComponent<MapInfo>();

        icon = GetComponent<Image>();
        node = GetComponent<Map.NodeMapInfo>();
        SetRoomType();
    }

    public void SetRoomType()
    {
        if (node.colIndex == 1)
        {
            roomType = normalEnemy;
            icon.sprite = roomType.roomIcon;
        }
        
        if (node.colIndex == 14)
        {
            roomType = rest;
            icon.sprite = roomType.roomIcon;
        }
        
        if (node.colIndex == 15)
        {
            roomType = boss;
            icon.sprite = roomType.roomIcon;
        }
    }

    public void FillData()
    {
        switch (roomType.Name){
            case RoomType.NormalEnemy:

            int randomIndex = Random.Range(0, map.selectedMap.normalEnemies.Count);

            characterOnScene = map.selectedMap.normalEnemies[randomIndex];
            break;
        }
        return;
    }
} 
