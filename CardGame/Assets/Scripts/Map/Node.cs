using UnityEngine;

namespace Map
{
    public class Node : MonoBehaviour
    {
        public int colIndex;
        public int rowIndex;
        public int nodeConnections;

        public void ConnectTo(Node otherNode)
        {
            LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
            Debug.Log($"aqui {gameObject.name}");
            Debug.Log($"aqui {otherNode.gameObject.name}");
            
            lineRenderer.SetPositions(new Vector3[] { transform.position, otherNode.transform.position });
        }
    }
}
