using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeGenerator : MonoBehaviour
{
    public GameObject startPointNode;
    public GameObject endPointNode;
    public Transform startPoint;
    public Transform endPoint;
    public GameObject nodePrefab;
    private float minDistance = 250f;

    private List<Vector3> gridPositions = new List<Vector3>();
    private List<GameObject> nodes = new List<GameObject>();
    [SerializeField] public NodesList nodesGrid = new NodesList();
    private int lastCol = 0;


    private void Start()
    {
        GameObjectList firstPosition = new GameObjectList();
        firstPosition.nodes.Add(startPointNode);
        nodesGrid.lists.Add(firstPosition);

        GenerateGrid();
        GenerateRandomNodes();
        GameObjectList lastPosition = new GameObjectList();
        lastPosition.nodes.Add(endPointNode);
        nodesGrid.lists.Add(lastPosition);

        CreateMap();
        
    }

    public void CreateMap()
    {
        int index;
        for (int i = 1; i <  nodesGrid.lists.Count; i++)
        {
            index = i - 1;
            ConnectNodes(nodesGrid.lists[index].nodes, nodesGrid.lists[i].nodes, index);
        }
    }

    private void GenerateGrid()
    {
        // Calculate the horizontal and vertical diameters of the rectangle
        float horizontalDiameter = Vector2.Distance(startPoint.position, endPoint.position);
        float verticalDiameter = horizontalDiameter / 2f;

        // Calculate the center of the rectangle
        Vector2 center = (startPoint.position + endPoint.position) / 2f;

        // Calculate the number of rows and columns in the grid
        int numRows = 5;
        int numCols = 14;

        // Calculate the spacing between nodes based on the desired minimum distance
        float nodeSpacing = minDistance + 50f;

        // Calculate the total width and height of the grid
        float totalWidth = (numCols - 1) * nodeSpacing;
        float totalHeight = (numRows - 1) * nodeSpacing;

        // Calculate the starting position for the first column
        float startX = center.x - totalWidth / 2f;
        float startY = center.y - totalHeight / 2f;

        // Loop to store the grid positions
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                // Calculate the position of the current node in the grid
                float xPos = startX + col * nodeSpacing;
                float yPos = startY + row * nodeSpacing;

                // Store the grid position in the list
                gridPositions.Add(new Vector3(xPos, yPos, 0f));
            }
        }
    }

    private void GenerateRandomNodes()
    {
        // Calculate the number of rows in the grid
        int numRows = 5;

        // Place nodes in the first column startPoint
        // nodes.Add(InstantiateNode(startPoint.position, 0, 2));

        for (int i = 1; i <= 3; i++)
        {
            Vector3 position = GetGridPosition(1, i);
            nodes.Add(InstantiateNode(position, 1, i));
        }

        // Fill the intermediate columns with nodes to ensure there is a path
        for (int col = 2; col <= 13; col++)
        {
            for (int row = 0; row < numRows; row++)
            {
                Vector3 position = GetGridPosition(col, row);
                nodes.Add(InstantiateNode(position, col, row));
            }
        }

        // Place nodes in the last column endPoint
        // nodes.Add(InstantiateNode(endPoint.position, 15, 2));

        for (int i = 1; i <= 3; i++)
        {
            Vector3 position = GetGridPosition(14, i);
            nodes.Add(InstantiateNode(position, 14, i));
        }
    }

    private Vector3 GetGridPosition(int col, int row)
    {
        int numCols = 14;
        return gridPositions[(row * numCols) + col - 1];
    }

    private GameObject InstantiateNode(Vector3 position, int col, int row)
    {
        if (col != lastCol)
        {
            lastCol = col;
            GameObjectList newNodesColumn = new GameObjectList();
            GameObject newNode = Instantiate(nodePrefab, position, Quaternion.identity);
            newNode.transform.SetParent(transform);

            Map.Node nodeComponent = newNode.GetComponent<Map.Node>();
            nodeComponent.colIndex = col;
            nodeComponent.rowIndex = row;

            newNode.name = "Node " + col + " " + row; // Renombramos el nodo con el formato deseado

            newNodesColumn.nodes.Add(newNode);
            nodesGrid.lists.Add(newNodesColumn);
            return newNode;
        }
        else
        {
            GameObject newNode = Instantiate(nodePrefab, position, Quaternion.identity);
            newNode.transform.SetParent(transform);

            Map.Node nodeComponent = newNode.GetComponent<Map.Node>();
            nodeComponent.colIndex = col;
            nodeComponent.rowIndex = row;

            newNode.name = "Node " + col + " " + row; // Renombramos el nodo con el formato deseado

            nodesGrid.lists[col].nodes.Add(newNode);

            return newNode;
        }
    }

    private void ConnectNodes(List<GameObject> sourceNodes, List<GameObject> targetNodes, int index)
    {
        // Variable to store the node with highest rowIndex from the previous iteration
        Map.Node previousHighestNode = null;

        foreach (var sourceNode in sourceNodes)
        {
            Map.Node sourceNodeComponent = sourceNode.GetComponent<Map.Node>();
            if (index == 0 || sourceNodeComponent.connected == true)
            {
                int numConnections;

                // Get the number of connections for the current node
                if (index != 0)
                {
                    sourceNodeComponent.GetRandomNumConnections(index);
                    numConnections = sourceNodeComponent.numConnections;
                }
                else
                {
                    numConnections = sourceNodeComponent.numConnections;
                }

                // Create a list to store the valid target nodes
                List<Map.Node> validTargetNodes = new List<Map.Node>();

                foreach (var targetNode in targetNodes)
                {
                    Map.Node targetNodeComponent = targetNode.GetComponent<Map.Node>();

                    // Check if the target node is within the valid range for connections
                    if (IsWithinValidRange(sourceNodeComponent, targetNodeComponent))
                    {
                        
                        // Check if the target node has a higher rowIndex than the previous highest node
                        if (previousHighestNode == null || targetNodeComponent.rowIndex >= previousHighestNode.rowIndex)
                        {
                            validTargetNodes.Add(targetNodeComponent);
                        }
                    }
                }

                // Connect the current node to the target nodes randomly
                for (int i = 0; i < numConnections; i++)
                {
                    // Check if there are valid target nodes to connect with
                    if (validTargetNodes.Count > 0)
                    {
                        // Choose a random target node from the validTargetNodes list
                        int randomIndex = Random.Range(0, validTargetNodes.Count);
                        Map.Node selectedNode = validTargetNodes[randomIndex];

                        // Connect the source node to the selected target node
                        sourceNodeComponent.ConnectTo(selectedNode);
                        // Debug.Log("Connected: " + sourceNode.name + " to " + selectedNode.name);

                        // Remove the selected node from the targetNodes list to avoid duplicate connections
                        validTargetNodes.Remove(selectedNode);
                    }
                }

                previousHighestNode = sourceNodeComponent.GetNodeWithHighestRowIndex();
            }
            else
            {
                Destroy(sourceNode);
            }

            
        }
    }

        // Helper function to check if the target node is within the valid range for connections
    private bool IsWithinValidRange(Map.Node sourceNode, Map.Node targetNode)
    {
        int colDiff = Mathf.Abs(targetNode.colIndex - sourceNode.colIndex);
        int rowDiff = Mathf.Abs(targetNode.rowIndex - sourceNode.rowIndex);

        return colDiff <= 1 && rowDiff <= 1 && colDiff + rowDiff > 0;
    }

    private bool IsNodeConnectedInNextColumn(Map.Node node, int nextColumnIndex)
    {
        foreach (var connectedNode in node.connectedNodes)
        {
            if (connectedNode.colIndex == nextColumnIndex)
            {
                return true;
            }
        }
        return false;
    }
}
