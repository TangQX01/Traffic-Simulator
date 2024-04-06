using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SIP.FastVRTools.Cameras
{
    //An fly camera can control using keyboard by W A S D and also can be use for touch screen.
    //One fingure is to rotate the camera, tow fingures are zoom in camera, and three fingures
    //are move camera left or right and four fingures are reset camera.
    //Edit by leo 2013-11-08

    [AddComponentMenu("SIP/FastVRTool/Cameras/Fly Camera")]

    public class FlyCamera : CameraBase
    {

        //--------------------------------  PC Variables  -----------------------------//
        public float m_pcSpeedScale = 50.0f;
        public float m_pcRotateScale = 30.0f;

        //-------------------------------- Ios Variables  -----------------------------//
        public float m_iosSpeedScale = 1.0f;
        public float m_iosRotateScale = 1.0f;

        [HideInInspector]
        public float m_lastTouchDistance = 0.0f;
        [HideInInspector]
        public float m_currentTouchDistance = 0.0f;
        [HideInInspector]
        public float m_touchDeltaDistance = 0.0f;

        //-------------------------------  Camera Variables  --------------------------//
        //Camera Accelaraters
        [HideInInspector]
        protected Vector3 m_rotSpeed = new Vector3(0.0f, 0.0f, 0.0f);
        [HideInInspector]
        protected Vector3 m_tranSpeed = new Vector3(0.0f, 0.0f, 0.0f);
        public bool m_isSmooth = false;
        public float m_moveAccelarater = 0.1f;
        public float m_rotateAccelarater = 0.1f;

        void Awake()
        {
            m_camType = CamType.Fly;
        }

        // Use this for initialization
        void Start()
        {
            Init();
        }

        // Update is called once per frame
        void Update()
        {
            CameraUpdate();
        }

        public override void CameraUpdate()
        {
			base.CameraUpdate();
			if(m_camType == CamType.Fly)
			{
				FlyControl();
			}
        }

        protected void FlyControl()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
            {
                //Single touch
                if (Input.touchCount == 1)
                {
                    //Only action when touch move
                    if (Input.touches[0].phase == TouchPhase.Moved)
                    {
                        m_rotSpeed.y = Input.touches[0].deltaPosition.x * m_camRotateSpeed * m_iosRotateScale;
                        m_rotSpeed.x = Input.touches[0].deltaPosition.y * m_camRotateSpeed * m_iosRotateScale;
                    }
                }

                //Double touch, Zoom in or zoom out and move leftright or updown
                if (Input.touchCount == 2)
                {
                    Touch touch1 = Input.GetTouch(0);
                    Touch touch2 = Input.GetTouch(1);

                    if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
                    {
                        m_currentTouchDistance = Vector2.Distance(touch1.position, touch2.position);
                        m_lastTouchDistance = Vector2.Distance((touch1.position - touch1.deltaPosition), (touch2.position - touch2.deltaPosition));
                        //Move Horizontal
                        if (m_currentTouchDistance - m_lastTouchDistance < 2.0f)
                        {
                            m_tranSpeed.y = Input.touches[1].deltaPosition.x * m_camHoriSpeed * m_iosSpeedScale;
                            m_tranSpeed.z = -Input.touches[1].deltaPosition.y * m_camHoriSpeed * m_iosSpeedScale;

                        }
                        //Zoom stuffs.
                        else
                        {
                            //Check if zoom in or zoom out
                            m_touchDeltaDistance = m_currentTouchDistance - m_lastTouchDistance;
                            m_tranSpeed.x = m_touchDeltaDistance * m_camForwardSpeed * m_iosSpeedScale;
                        }
                    }
                }

                //Three touch, reset camera
                if (Input.touchCount == 3)
                {
                    transform.position = m_camPosOrigin;
                    transform.rotation = m_camQuatOrigin;
                }
            }
            else
            {
                //Translate control
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                {
                    m_tranSpeed.x = m_camForwardSpeed * m_pcSpeedScale;
                }

                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    m_tranSpeed.x = -m_camForwardSpeed * m_pcSpeedScale;
                }

                if (Input.GetKey(KeyCode.A))
                {
                    m_tranSpeed.y = m_camHoriSpeed * m_pcSpeedScale;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    m_tranSpeed.y = -m_camHoriSpeed * m_pcSpeedScale;
                }

                if (Input.GetKey(KeyCode.Q))
                {
                    m_tranSpeed.z = m_camHoriSpeed * m_pcSpeedScale;
                }

                if (Input.GetKey(KeyCode.E))
                {
                    m_tranSpeed.z = -m_camHoriSpeed * m_pcSpeedScale;
                }

                //Rotation controls
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    m_rotSpeed.y = -m_camRotateSpeed * m_pcRotateScale;
                }

                if (Input.GetKey(KeyCode.RightArrow))
                {
                    m_rotSpeed.y = m_camRotateSpeed * m_pcRotateScale;
                }

                if (Input.GetMouseButton(0))
                {
                    m_rotSpeed.x = -Input.GetAxis("Mouse Y") * m_camRotateSpeed * m_pcRotateScale;
                    m_rotSpeed.y = Input.GetAxis("Mouse X") * m_camRotateSpeed * m_pcRotateScale;
                }

                //Move Horizontal
                if (Input.GetMouseButton(2))
                {
                    m_tranSpeed.z = -Input.GetAxis("Mouse Y") * m_pcSpeedScale; ;
                    m_tranSpeed.y = Input.GetAxis("Mouse X") * m_pcSpeedScale; ;
                }

                //Zoom in or out
                if (Input.GetAxis("Mouse ScrollWheel") != 0)
                {
                    m_tranSpeed.x = Input.GetAxis("Mouse ScrollWheel") * m_pcSpeedScale; ;
                }

                //Reset transform
                if (Input.GetKey(KeyCode.R))
                {
                    transform.position = m_camPosOrigin;
                    transform.rotation = m_camQuatOrigin;
                }
            }


            //Smooth move and rotate
            if (m_isSmooth)
            {
                if (m_tranSpeed.x > 0)
                {
                    m_tranSpeed.x -= m_moveAccelarater;
                    if (m_tranSpeed.x < 0)
                        m_tranSpeed.x = 0;

                    transform.Translate(Vector3.forward * m_tranSpeed.x * Time.deltaTime);
                }
                else if (m_tranSpeed.x < 0)
                {
                    m_tranSpeed.x += m_moveAccelarater;

                    if (m_tranSpeed.x > 0)
                        m_tranSpeed.x = 0;

                    transform.Translate(Vector3.forward * m_tranSpeed.x * Time.deltaTime);
                }

                if (m_tranSpeed.y > 0)
                {
                    m_tranSpeed.y -= m_moveAccelarater;

                    if (m_tranSpeed.y < 0)
                        m_tranSpeed.y = 0;

                    transform.Translate(Vector3.left * m_tranSpeed.y * Time.deltaTime);
                }
                else if (m_tranSpeed.y < 0)
                {
                    m_tranSpeed.y += m_moveAccelarater;

                    if (m_tranSpeed.y > 0)
                        m_tranSpeed.y = 0;

                    transform.Translate(Vector3.left * m_tranSpeed.y * Time.deltaTime);
                }

                if (m_tranSpeed.z > 0)
                {
                    m_tranSpeed.z -= m_moveAccelarater;

                    if (m_tranSpeed.z < 0)
                        m_tranSpeed.z = 0;

                    transform.Translate(Vector3.up * m_tranSpeed.z * Time.deltaTime);
                }
                else if (m_tranSpeed.z < 0)
                {
                    m_tranSpeed.z += m_moveAccelarater;

                    if (m_tranSpeed.z > 0)
                        m_tranSpeed.z = 0;

                    transform.Translate(Vector3.up * m_tranSpeed.z * Time.deltaTime);
                }

                //Smooth Rotate

                if (m_rotSpeed.x > 0)
                {
                    m_rotSpeed.x -= m_rotateAccelarater;

                    if (m_rotSpeed.x < 0)
                        m_rotSpeed.x = 0;

                    transform.Rotate(-Vector3.left * m_rotSpeed.x * Time.deltaTime, Space.Self);
                }
                else if (m_rotSpeed.x < 0)
                {
                    m_rotSpeed.x += m_rotateAccelarater;

                    if (m_rotSpeed.x > 0)
                        m_rotSpeed.x = 0;

                    transform.Rotate(-Vector3.left * m_rotSpeed.x * Time.deltaTime, Space.Self);
                }

                if (m_rotSpeed.y > 0)
                {
                    m_rotSpeed.y -= m_rotateAccelarater;

                    if (m_rotSpeed.y < 0)
                        m_rotSpeed.y = 0;

                    transform.Rotate(-Vector3.up * m_rotSpeed.y * Time.deltaTime, Space.World);
                }
                else if (m_rotSpeed.y < 0)
                {
                    m_rotSpeed.y += m_rotateAccelarater;

                    if (m_rotSpeed.y > 0)
                        m_rotSpeed.y = 0;

                    transform.Rotate(-Vector3.up * m_rotSpeed.y * Time.deltaTime, Space.World);
                }
            }
            else
            {
                transform.Translate(Vector3.forward * m_tranSpeed.x * Time.deltaTime);
                transform.Translate(Vector3.left * m_tranSpeed.y * Time.deltaTime);
                transform.Translate(Vector3.up * m_tranSpeed.z * Time.deltaTime);

                transform.Rotate(-Vector3.left * m_rotSpeed.x * Time.deltaTime, Space.Self);
                transform.Rotate(Vector3.up * m_rotSpeed.y * Time.deltaTime, Space.World);

                m_tranSpeed = new Vector3(0.0f, 0.0f, 0.0f);
                m_rotSpeed = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }

        void OnGUI()
        {
            UpdateGUI();
        }
    }
}
