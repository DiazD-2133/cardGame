using UnityEngine;

public class NodeClickHandler : MonoBehaviour
{
    public void OnNodeClick()
    {
        Debug.Log($"{gameObject.name} clicked!");
    }
    
}
