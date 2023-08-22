using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class NodeMapInfo : MonoBehaviour
    {
        
        public GameObject lineRendererObject;
        public GameObject lineRendererStartEndPrefab;

        public int colIndex;
        public int rowIndex;
        public int numConnections;
        public bool connected;
        public List<NodeMapInfo> connectedNodes = new List<NodeMapInfo>();
        public List<UnityEngine.UI.Extensions.UILineRenderer> paths = new List<UnityEngine.UI.Extensions.UILineRenderer>();
        private GameObject newLine;

        public void GetRandomNumConnections(int index)
        {
            if(index == 14)
            {
                numConnections = 1;
            }
            else if(index < 4 && index > 14)
            {
                numConnections = Random.Range(1, 4);
            }
            else
            {
                numConnections = Random.Range(1, 3);
            }
        }

        public void ConnectTo(NodeMapInfo otherNode, bool inList = false)
        {
            if (!inList)
            {
                newLine = Instantiate(lineRendererObject);
            }
            else if (inList && Screen.height >= 1440)
            {
                newLine = Instantiate(lineRendererObject);
            } 
            else 
            {
                newLine = Instantiate(lineRendererStartEndPrefab);
            }
            newLine.transform.SetParent(transform);
            UnityEngine.UI.Extensions.UILineRenderer lineRenderer = newLine.GetComponent<UnityEngine.UI.Extensions.UILineRenderer>();
            Vector3 startPosition = new Vector3(transform.position.x + 26f, transform.position.y) ;
            Vector3 endPosition = otherNode.transform.position;
            lineRenderer.Points = new Vector2[] { startPosition, endPosition };
            // lineRenderer.color = Color.red;
            lineRenderer.connectedTo = otherNode;
            paths.Add(lineRenderer);

            connected = true;
            otherNode.connected = true;

            if (!inList)
            {
                connectedNodes.Add(otherNode);
            }
        }

        public NodeMapInfo GetNodeWithHighestRowIndex()
        {
            NodeMapInfo nodeWithHighestRowIndex = null;
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

        void OnDestroy()
        {
            foreach(var lineRenderer in paths)
            {
                Destroy(lineRenderer);
            }
        }
    }
}
