using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodesList
{
    public List<GameObjectList> lists;
}

    // Start is called before the first frame update
[System.Serializable]
public class GameObjectList
{
    public List<GameObject> nodes = new List<GameObject>();
}

