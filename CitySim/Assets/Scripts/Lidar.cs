using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lidar : MonoBehaviour
{
    [Tooltip("���߸���")]
    [SerializeField] private int rayNum = 16;
    [Tooltip("�����״������Ч����")]
    [SerializeField] private float sensorLength = 15f;
    [Tooltip("��������Ƕ�")]
    [SerializeField] private float angle = 30;
    [Tooltip("��ת�ǶȲ���")]
    [SerializeField] private float stepAngle = 1f;


    [Tooltip("�Ƿ���ʾ����")]
    [SerializeField] private bool showRay = true;
    [Tooltip("������ɫ")]
    [SerializeField] private Color rayColor = Color.blue;

    private Vector3 direction;  //Ŀǰ�����߷���
    private Ray ray;    //Ŀǰ����
    public RaycastHit hit; //����ɨ���ĵ�

    void FixedUpdate()
    {
        //print("Begin");
        //ɨ��
        Scan(); //��ǰ��������

        //��ת����Ƕ�
        //transform.rotation = Quaternion.AngleAxis(currentRotAngle, transform.up);  //��ת�쳣
        //currentRotAngle += stepAngle;
        //if(currentRotAngle >= 360) { currentRotAngle = 0f; }
        transform.Rotate(0, stepAngle, 0, Space.Self);
    }

    //ɨ��
    void Scan()
    {

        for (int i = 0; i < rayNum; i++)
        {
            direction = Quaternion.AngleAxis(angle * i / rayNum, transform.right) * transform.forward;
            ray = new Ray(transform.position, direction);
            if (Physics.Raycast(ray, out hit, sensorLength))
            {
                if (showRay) //���߿��ӻ�
                {
                    Debug.DrawLine(transform.position, hit.point, rayColor);
                }
                Debug.Log(hit.point);
            }
        }
    }
}

