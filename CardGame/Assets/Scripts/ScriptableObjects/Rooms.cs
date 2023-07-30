using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public enum RoomType
    {
        NormalEnemy,
        EliteEnemy,
        Boss,
        Event,
        Rest,
        Chest,
        Start,
    }

[CreateAssetMenu( fileName = "New Room", menuName = "Rooms")]
public class Rooms : ScriptableObject
{
    public RoomType Name;
    public Sprite roomIcon;
}
