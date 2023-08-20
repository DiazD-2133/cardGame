using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapInfo : MonoBehaviour
{
    // Came from selection
    public EnemiesList selectedMap;
    // Came from selection
    public string stageName;

    public Map.NodeMapInfo startNode;
    private Color lineColor;
    public Map.NodeMapInfo playerPosition;

    public void ChangePlayerPosition(Map.NodeMapInfo nodeInfo)
    {
        if (playerPosition != null)
        {
            for (int i = 0; i < playerPosition.connectedNodes.Count; i++)
            {
                
                Button selectableNode = playerPosition.connectedNodes[i].GetComponent<Button>();
                if (selectableNode!= null && playerPosition.connectedNodes[i] != nodeInfo)
                {
                    selectableNode.interactable = false;
                }
                else if(playerPosition.connectedNodes[i] == nodeInfo)
                {
                    selectableNode.enabled = false;
                }

                if (playerPosition.paths[i].connectedTo == nodeInfo){
                    playerPosition.paths[i].color = lineColor;
                }
            }
        }
        playerPosition = nodeInfo;
    }

    public void NextPositions(Map.NodeMapInfo nodeData)
    {
        foreach(var node in nodeData.connectedNodes)
        {
            Button selectableNode = node.GetComponent<Button>();
            if (selectableNode!= null)
            {
                selectableNode.interactable = true;
            } 
            
        }
    }

    public void InitializePlayerPosition()
    {
        lineColor = startNode.GetComponent<Image>().color;
        ChangePlayerPosition(startNode);
        NextPositions(startNode);
    }
}
