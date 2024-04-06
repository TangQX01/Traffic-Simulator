
using UnityEngine;

[RequireComponent(typeof(Camera))]
//按住鼠标右键旋转，同时按wasd移动，按住shift加速移动，按住中键拖拽视图
public class CameraControl : MonoBehaviour
{

    //相机旋转速度
    public float rotateSpeed = 5f;
    //相机缩放速度
    public float scaleSpeed = 10f;

    //旋转变量
    private float m_deltX = 0f;
    private float m_deltY = 0f;

    //移动变量
    float m_camNormalMoveSpeed = 0.2f;
    float m_camFastMoveSpeed = 2f;
    private Vector3 m_mouseMovePos = Vector3.zero;
    private Vector3 m_targetPos;
    Camera m_cam;
    float m_distance;
    float m_camHitDistance = 10;
    Quaternion m_camBeginRotation;

    void Start()
    {
        m_cam = GetComponent<Camera>();
        m_camBeginRotation = m_cam.transform.rotation;
    }

    void Update()
    {

        if (Input.GetMouseButton(1))
        {
            //鼠标右键点下控制相机旋转;
            m_deltX += Input.GetAxis("Mouse X") * rotateSpeed;
            m_deltY -= Input.GetAxis("Mouse Y") * rotateSpeed;
            m_deltX = ClampAngle(m_deltX, -360, 360);
            m_deltY = ClampAngle(m_deltY, -70, 70);
            m_cam.transform.rotation = m_camBeginRotation * Quaternion.Euler(m_deltY, m_deltX, 0);

            //鼠标右键按住时控制相机移动
            float _inputX = Input.GetAxis("Horizontal");
            float _inputY = Input.GetAxis("Vertical");
            float _camMoveSpeed = Input.GetKey(KeyCode.LeftShift) ? m_camFastMoveSpeed : m_camNormalMoveSpeed;
            _camMoveSpeed *= 8;
            m_targetPos = transform.position + transform.forward * _camMoveSpeed * _inputY + transform.right * _camMoveSpeed * _inputX;
            transform.position = Vector3.Lerp(transform.position, m_targetPos, 0.5f);

        }

        //鼠标中键点下场景缩放
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            m_distance = Input.GetAxis("Mouse ScrollWheel") * scaleSpeed;
            m_targetPos = m_cam.transform.position + m_cam.transform.forward * m_distance;
            m_cam.transform.position = Vector3.Lerp(m_cam.transform.position, m_targetPos, 0.5f);
        }

        //鼠标拖拽视野
        if (Input.GetMouseButtonDown(2))
        {
            //跟手拖拽的关键
            Ray ray = m_cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Vector3 _offset = hit.point - transform.position;
                this.m_camHitDistance = Vector3.Dot(_offset, transform.forward);
            }
            m_mouseMovePos = m_cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_camHitDistance));
        }
        else if (Input.GetMouseButton(2))
        {
            Vector3 _worldPoint = Vector3.zero;
            _worldPoint = m_cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_camHitDistance)) - m_mouseMovePos;
            m_cam.transform.position = m_cam.transform.position - _worldPoint;
        }
    }

    float ClampAngle(float angle, float minAngle, float maxAgnle)
    {
        if (angle <= -360)
            angle += 360;
        if (angle >= 360)
            angle -= 360;

        return Mathf.Clamp(angle, minAngle, maxAgnle);
    }
}