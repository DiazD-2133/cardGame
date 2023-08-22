using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GenerateRandomMap : MonoBehaviour
{
    private GameObject startPointNode;
    public GameObject endPointNode;
    [SerializeField] private RectTransform mapWidth;
    [SerializeField] private Transform middleNodesPosition;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private MapInfo mapInfo;
    public GameObject nodePrefab;
    public GameObject startPositionNodePrefab;

    private RectTransform canvasRectTransform;
    private float minDistance = 300f;
    private List<Vector3> gridPositions = new List<Vector3>();
    private List<GameObject> nodes = new List<GameObject>();
    private List<GameObject> nodesWithOutPaths = new List<GameObject>();
    [SerializeField] public NodesList nodesGrid = new NodesList();
    private int lastCol = 0;


    private void Start()
    {
    }

    public void GenerateMap()
    {
        RectTransform middleNodesScale = middleNodesPosition.GetComponent<RectTransform>();
        middleNodesScale.localScale = Vector3.one;
        startPoint.localScale = Vector3.one;

        canvasRectTransform = GameObject.Find("Game UI").GetComponent<RectTransform>();
        Vector3 canvasScale = canvasRectTransform.localScale;

        GameObjectList firstPosition = new GameObjectList();
        startPointNode = Instantiate(startPositionNodePrefab, startPoint.transform.position, Quaternion.identity);
        startPointNode.transform.SetParent(startPoint);
        mapInfo.startNode = startPointNode.GetComponent<Map.NodeMapInfo>();
        firstPosition.nodes.Add(startPointNode);
        nodesGrid.lists.Add(firstPosition);
        GenerateGrid();
        GenerateRandomNodes();
        GameObjectList lastPosition = new GameObjectList();
        lastPosition.nodes.Add(endPointNode);
        nodesGrid.lists.Add(lastPosition);

        CreateMap();
        
        middleNodesScale.localScale = canvasScale;

        GeneratePaths(nodesWithOutPaths);

        nodesWithOutPaths.Clear();

    }

    public void ClearMap()
    {
        gridPositions.Clear();
        
        if (startPointNode != null)
        {
            Map.NodeMapInfo startNodeComponent = startPointNode.GetComponent<Map.NodeMapInfo>();
            startNodeComponent.connectedNodes.Clear();

            DestroyPaths(startPointNode);
        }

        foreach(var node in nodes){
            Destroy(node);
        }

        nodes.Clear();
        nodesGrid.lists.Clear();
    }

    private void GeneratePaths(List<GameObject> nodesWithOutPaths)
    {
        bool inList = true;
        foreach(var node in nodesWithOutPaths)
        {
            Map.NodeMapInfo sourceNodeComponent = node.GetComponent<Map.NodeMapInfo>();
            for(int i = 0; i < sourceNodeComponent.numConnections; i++)
            {
                sourceNodeComponent.ConnectTo(sourceNodeComponent.connectedNodes[i], inList);
            }
        }
    }

    private void DestroyPaths(GameObject node)
    {
        Map.NodeMapInfo nodePositionPaths = node.GetComponent<Map.NodeMapInfo>(); 

        foreach(var path in nodePositionPaths.paths){
            Destroy(path.gameObject);
        }

        nodePositionPaths.paths.Clear();
    }

    private void CreateMap()
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
        // float horizontalDiameter = Vector2.Distance(startPoint.position, endPoint.position);
        // float verticalDiameter = horizontalDiameter / 2f;

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

        // Define the range for random offsets in x and y positions
        float minXOffset = -90f;
        float maxXOffset = 90f;
        float minYOffset = -80f;
        float maxYOffset = 80f;

        // Loop to store the grid positions
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                // Calculate the position of the current node in the grid with random offsets
                float xPos = startX + col * nodeSpacing + Random.Range(minXOffset, maxXOffset);
                float yPos = startY + row * nodeSpacing + Random.Range(minYOffset, maxYOffset);

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
        GameObject newNode = Instantiate(nodePrefab, position, Quaternion.identity);
        newNode.transform.SetParent(middleNodesPosition);

        Map.NodeMapInfo nodeComponent = newNode.GetComponent<Map.NodeMapInfo>();
        nodeComponent.colIndex = col;
        nodeComponent.rowIndex = row;

        newNode.name = "Node " + col + " " + row;
        
        if (col != lastCol)
        {
            lastCol = col;
            GameObjectList newNodesColumn = new GameObjectList();

            newNodesColumn.nodes.Add(newNode);
            nodesGrid.lists.Add(newNodesColumn);
            return newNode;
        }
        else
        {
            nodesGrid.lists[col].nodes.Add(newNode);

            return newNode;
        }
    }

    private void ConnectNodes(List<GameObject> sourceNodes, List<GameObject> targetNodes, int index)
    {
        // Variable to store the node with highest rowIndex from the previous iteration
        Map.NodeMapInfo previousHighestNode = null;

        foreach (var sourceNode in sourceNodes)
        {
            Map.NodeMapInfo sourceNodeComponent = sourceNode.GetComponent<Map.NodeMapInfo>();
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
                List<Map.NodeMapInfo> validTargetNodes = new List<Map.NodeMapInfo>();

                foreach (var targetNode in targetNodes)
                {
                    Map.NodeMapInfo targetNodeComponent = targetNode.GetComponent<Map.NodeMapInfo>();

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
                        Map.NodeMapInfo selectedNode = validTargetNodes[randomIndex];
                        
                        // Connect the source node to the selected target node
                        sourceNodeComponent.ConnectTo(selectedNode);
                        // Debug.Log("Connected: " + sourceNode.name + " to " + selectedNode.name);

                        // Remove the selected node from the targetNodes list to avoid duplicate connections
                        validTargetNodes.Remove(selectedNode);
                    }
                }

                if (index == 0 || index == nodesGrid.lists.Count - 2)
                {
                    DestroyPaths(sourceNode);
                    nodesWithOutPaths.Add(sourceNode);
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
    private bool IsWithinValidRange(Map.NodeMapInfo sourceNode, Map.NodeMapInfo targetNode)
    {
        int colDiff = Mathf.Abs(targetNode.colIndex - sourceNode.colIndex);
        int rowDiff = Mathf.Abs(targetNode.rowIndex - sourceNode.rowIndex);

        return colDiff <= 1 && rowDiff <= 1 && colDiff + rowDiff > 0;
    }
}
