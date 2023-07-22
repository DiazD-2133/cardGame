using UnityEngine;

public class NodeClickHandler : MonoBehaviour
{
    public Map.Node nodeInfo;
    public void OnNodeClick()
    {
        Debug.Log($"Name: {gameObject.name}, Connections: {nodeInfo.numConnections} ");
    }
    
}
