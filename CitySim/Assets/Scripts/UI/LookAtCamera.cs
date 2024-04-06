using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SIP.FastVRTools;

public class LookAtCamera : MonoBehaviour
{
    public FVRCameraManager m_CameraManager;
    private Camera m_Camera;
    public float minSize = 1f; // 最小大小
    public float maxSize = 10f; // 最大大小
    private Vector3 m_InitialSize;
    // Start is called before the first frame update
    void Start()
    {
        m_InitialSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        m_Camera = m_CameraManager.m_cameras[m_CameraManager.m_currentCamNum];
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward, m_Camera.transform.rotation * Vector3.up);
        AdjustSize();
    }

    public void AdjustSize() {
        // 获取相机位置和物体位置
        Vector3 cameraPos = m_Camera.transform.position;
        Vector3 targetPos = transform.position;

        // 计算相机到物体的距离
        float distance = Vector3.Distance(cameraPos, targetPos);

        // 根据距离调整物体的大小
        // float scale = Mathf.Clamp(distance, minSize, maxSize);
        float scale = 1.0f * distance / 600;
        transform.localScale = m_InitialSize * scale;
    }
}
