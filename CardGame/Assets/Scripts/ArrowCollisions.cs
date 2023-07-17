using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowCollisions : MonoBehaviour
{
    public GameObject enemy;
    public List<GameObject> nodesList = new List<GameObject>();
    public bool IsColliding;
    private Color originalColor;



    // Start is called before the first frame update
    void Start()
    {
        originalColor = GetComponent<Image>().color;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        IsColliding = true; 
        enemy = other.gameObject;

        GetComponent<Image>().color = Color.red;

        foreach (GameObject node in nodesList)
        {
            node.GetComponent<Image>().color = Color.red;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        GetComponent<Image>().color = originalColor;

        foreach (GameObject node in nodesList)
        {
            node.GetComponent<Image>().color = originalColor;
        }

        IsColliding = false; 
    }
}
