using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lidar : MonoBehaviour
{
    [Tooltip("射线根数")]
    [SerializeField] private int rayNum = 16;
    [Tooltip("激光雷达最大有效距离")]
    [SerializeField] private float sensorLength = 15f;
    [Tooltip("激光纵向角度")]
    [SerializeField] private float angle = 30;
    [Tooltip("旋转角度步长")]
    [SerializeField] private float stepAngle = 1f;


    [Tooltip("是否显示射线")]
    [SerializeField] private bool showRay = true;
    [Tooltip("射线颜色")]
    [SerializeField] private Color rayColor = Color.blue;

    private Vector3 direction;  //目前的射线方向
    private Ray ray;    //目前射线
    public RaycastHit hit; //射线扫到的点

    void FixedUpdate()
    {
        //print("Begin");
        //扫描
        Scan(); //向前发出射线

        //旋转自身角度
        //transform.rotation = Quaternion.AngleAxis(currentRotAngle, transform.up);  //旋转异常
        //currentRotAngle += stepAngle;
        //if(currentRotAngle >= 360) { currentRotAngle = 0f; }
        transform.Rotate(0, stepAngle, 0, Space.Self);
    }

    //扫描
    void Scan()
    {

        for (int i = 0; i < rayNum; i++)
        {
            direction = Quaternion.AngleAxis(angle * i / rayNum, transform.right) * transform.forward;
            ray = new Ray(transform.position, direction);
            if (Physics.Raycast(ray, out hit, sensorLength))
            {
                if (showRay) //射线可视化
                {
                    Debug.DrawLine(transform.position, hit.point, rayColor);
                }
                Debug.Log(hit.point);
            }
        }
    }
}

