
using UnityEngine;

[RequireComponent(typeof(Camera))]
//��ס����Ҽ���ת��ͬʱ��wasd�ƶ�����סshift�����ƶ�����ס�м���ק��ͼ
public class CameraControl : MonoBehaviour
{

    //�����ת�ٶ�
    public float rotateSpeed = 5f;
    //��������ٶ�
    public float scaleSpeed = 10f;

    //��ת����
    private float m_deltX = 0f;
    private float m_deltY = 0f;

    //�ƶ�����
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
            //����Ҽ����¿��������ת;
            m_deltX += Input.GetAxis("Mouse X") * rotateSpeed;
            m_deltY -= Input.GetAxis("Mouse Y") * rotateSpeed;
            m_deltX = ClampAngle(m_deltX, -360, 360);
            m_deltY = ClampAngle(m_deltY, -70, 70);
            m_cam.transform.rotation = m_camBeginRotation * Quaternion.Euler(m_deltY, m_deltX, 0);

            //����Ҽ���סʱ��������ƶ�
            float _inputX = Input.GetAxis("Horizontal");
            float _inputY = Input.GetAxis("Vertical");
            float _camMoveSpeed = Input.GetKey(KeyCode.LeftShift) ? m_camFastMoveSpeed : m_camNormalMoveSpeed;
            _camMoveSpeed *= 8;
            m_targetPos = transform.position + transform.forward * _camMoveSpeed * _inputY + transform.right * _camMoveSpeed * _inputX;
            transform.position = Vector3.Lerp(transform.position, m_targetPos, 0.5f);

        }

        //����м����³�������
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            m_distance = Input.GetAxis("Mouse ScrollWheel") * scaleSpeed;
            m_targetPos = m_cam.transform.position + m_cam.transform.forward * m_distance;
            m_cam.transform.position = Vector3.Lerp(m_cam.transform.position, m_targetPos, 0.5f);
        }

        //�����ק��Ұ
        if (Input.GetMouseButtonDown(2))
        {
            //������ק�Ĺؼ�
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