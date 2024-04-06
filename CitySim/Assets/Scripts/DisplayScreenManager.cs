using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayScreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string intfoText = "当前获得屏幕数量为：" + Display.displays.Length;
        print(intfoText);
    }
    void Awake()
    {
        Display.displays[2].Activate();
        //for (int i = 0; i < Display.displays.Length; i++)
        //{
        //    Display.displays[i].Activate();
        //    Screen.SetResolution(Display.displays[i].renderingWidth, Display.displays[i].renderingHeight, true);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
