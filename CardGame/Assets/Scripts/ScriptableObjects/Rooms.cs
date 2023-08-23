using UnityEngine;

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
    public RoomType roomName;
    public Sprite roomIcon;
}
