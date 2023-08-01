using UnityEngine;

public class NodeClickHandler : MonoBehaviour
{
    private ShowMap map;
    private SceneConstructor scene;
    [SerializeField] private Map.NodeMapInfo nodeInfo;

    void Start()
    {
       GameObject gameManager = GameObject.Find("Game Manager");
       map = GameObject.Find("Map Button").GetComponent<ShowMap>();
       scene = gameManager.GetComponent<SceneConstructor>();

    }

    public void OnNodeClick()
    {
        NodeInfo nodeData = nodeInfo.GetComponent<NodeInfo>();
        scene.nodeData = nodeData;
        scene.NodeTypeSceneCreator();
        map.CloseMap();
        MapInfo sceneMap = map.map.GetComponent<MapInfo>();
        sceneMap.ChangePlayerPosition(nodeInfo);
        sceneMap.NextPositions(nodeInfo);
    }
    
}
