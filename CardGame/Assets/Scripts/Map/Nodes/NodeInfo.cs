using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeInfo : MonoBehaviour
{
    public List<Rooms> roomTypesList = new List<Rooms>();
    public Rooms roomType;
    private Image icon;
    [SerializeField] private Map.NodeMapInfo node;
    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponent<Image>();
        icon.sprite = roomType.roomIcon;
    }

    public void SetRoomType()
    {

    }
}
