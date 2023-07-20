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

        ConnectNodes(nodesGrid.lists[0].nodes, nodesGrid.lists[1].nodes);
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

    private void ConnectNodes(List<GameObject> sourceNodes, List<GameObject> targetNodes)
    {
        foreach (var sourceNode in sourceNodes)
        {
            Debug.Log(sourceNode.name);
            foreach (var targetNode in targetNodes)
            {
                Debug.Log(targetNode.name);
                
                Map.Node sourceNodeComponent = sourceNode.GetComponent<Map.Node>();
                Map.Node targetNodeComponent = targetNode.GetComponent<Map.Node>();

                // Por ejemplo, aquí estableceremos una conexión entre todos los nodos.
                sourceNodeComponent.ConnectTo(targetNodeComponent);
            }
        }
    }
}
