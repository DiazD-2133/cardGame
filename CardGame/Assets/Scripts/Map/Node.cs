using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class Node : MonoBehaviour
    {
        
        public GameObject lineRendererObject;
        public int colIndex;
        public int rowIndex;
        public int numConnections;
        public bool connected;
        public List<Node> connectedNodes = new List<Node>();

        public void GetRandomNumConnections(int index)
        {
            if(index < 6)
            {
                numConnections = Random.Range(1, 4);
            }
            else
            {
                numConnections = Random.Range(1, 3);
            }
        }

        public void ConnectTo(Node otherNode)
        {
            GameObject newLine = Instantiate(lineRendererObject);
            newLine.transform.SetParent(transform);
            UnityEngine.UI.Extensions.UILineRenderer lineRenderer = newLine.GetComponent<UnityEngine.UI.Extensions.UILineRenderer>();
            Vector3 startPosition = transform.position;
            Vector3 endPosition = otherNode.transform.position;
            lineRenderer.Points = new Vector2[] { startPosition, endPosition };

            connected = true;
            otherNode.connected = true;

            connectedNodes.Add(otherNode);
        }

        public Node GetNodeWithHighestRowIndex()
        {
            Node nodeWithHighestRowIndex = null;
            int highestRowIndex = int.MinValue;

            foreach (var node in connectedNodes)
            {
                if (node.rowIndex > highestRowIndex)
                {
                    highestRowIndex = node.rowIndex;
                    nodeWithHighestRowIndex = node;
                }
            }

            return nodeWithHighestRowIndex;
        }
    }
}
