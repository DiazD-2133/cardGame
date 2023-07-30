using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMap : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject modal;
    public GameObject map;
    private bool isOpen = false;

    public void OpenCloseMap()
    {
        isOpen = IsOpen();
        
        modal.SetActive(isOpen);
        map.SetActive(isOpen);
    }

    private bool IsOpen()
    {
        if(!isOpen)
        {
            isOpen = true;
            return isOpen;
        }

        isOpen = false;
        return isOpen;
    }

    public void CloseMap()
    {
        modal.SetActive(false);
        map.SetActive(false);
        isOpen = false;
    }
}
