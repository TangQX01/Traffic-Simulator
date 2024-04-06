using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatMapPanelManager : MonoBehaviour
{
    public GameObject HeatMap;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            HeatMap.SetActive(!HeatMap.activeSelf);
        }
    }
}
