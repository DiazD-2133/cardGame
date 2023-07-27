using UnityEngine;

public class NodeClickHandler : MonoBehaviour
{
    [SerializeField] private Map.NodeMapInfo nodeInfo;
    public void OnNodeClick()
    {
        Debug.Log($"Name: {gameObject.name}, Connections: {nodeInfo.numConnections} ");
    }
    
}
