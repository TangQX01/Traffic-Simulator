using UnityEngine;
using System.Collections;

namespace SIP.FastVRTools.Cameras
{
    public enum FollowCameraStatus
    {
        Follow,
        Auto_Around_Target,
        Transform_Cam,
        Free
    }

    [RequireComponent(typeof(Camera))]

    [AddComponentMenu("SIP/FastVRTool/Cameras/Follow Camera")]
    public class FollowCamera : FlyCamera
    {

        //Follow Properties
        /// <summary>
        /// This is the followed obj.
        /// </summary>
        public GameObject m_followObject = null;

        Bounds m_objBound = new Bounds();
        public FollowCameraStatus m_status = FollowCameraStatus.Follow;	//Follow camera.

        //Translate stuffs.
        public bool m_maxDistanceEnable = false;		//Enable max distance from object
        public float m_maxDistance = 100.0f;			//Max distance from object.
        public bool m_minDistanceEnable = false;		//Enable min distance from object
        public float m_minDistance = 10.0f;				//Min distance from object.
        public float m_bestDistance = 0.0f;				//Best view distance from object.
        [HideInInspector]
        public Vector3 m_bestPos = new Vector3(0, 0, 0);//Best position at best distance.

        /// <summary>
        /// This is to record the last transform of the object.
        /// </summary>
        private Vector3 m_followObjLastPos = new Vector3(0, 0, 0);

        /// <summary>
        /// This is to record the last rotation of the object.
        /// </summary>
        private Vector3 m_followObjLastRot = new Vector3(0, 0, 0);

        //[HideInInspector]
        public float m_currentDistance = 0.0f;			//Current view disntance

        //Rotate stuffs.
        public bool m_maxVertialEnable = false;			//Enable max vertical angle limits.
        public float m_maxVerticalAngle = 45.0f;		//Max vertical view angle
        public bool m_minVertialEnable = false;			//Enable min vertical angle limits.
        public float m_minVertialAngle = -45.0f;		//Min vertical view angle
        float m_verticalAngle = 0.0f;
        [HideInInspector]
        public Vector3 m_currentDir = new Vector3(0, 0, 0);//Best look at direction at best distance.

        //Auto around property.
        public bool m_enableStop = false;				//Enable stop
        public float m_autoRotateSpeed = 10.0f;			//Auto Rotate speed.
        public float m_autoStopTime = 10.0f;			//Auto rorate total stop time.
        float m_currentStopTime = 0.0f;					//Actived stop Time.
        [HideInInspector]
        public bool m_isAutoStop = false;				//Is Auto Rotate Stop
        public bool m_AutoRotateInverse = false;		//Rotate inverse

        public bool m_isUpdate = true;                 //Not Update

        void Awake()
        {
            Init();
            m_camType = CamType.Follow;

            if (m_followObject)
            {
                m_currentDistance = Vector3.Distance(transform.position, m_followObject.transform.position);
                m_followObjLastPos = m_followObject.transform.position;
                m_followObjLastRot = m_followObject.transform.rotation.eulerAngles;
            }

        }

        // Use this for initialization
        void Start()
        {
            if (m_followObject != null)
            {
                //Bounds bounds = m_followObject.renderer.bounds;
                //m_objBound = m_followObject.renderer.bounds;
                //m_objBound = m_followObject.GetComponentsInChildren<MeshFilter>().mesh.bounds;
                //Debug.Log(m_objBound);
            }

            //Check limits to avoid error by human change.
            if (m_maxDistanceEnable && m_minDistanceEnable)
            {
                if (m_maxDistance < m_minDistance)
                    m_maxDistance = m_minDistance;
            }

            if (m_maxVertialEnable && m_minVertialEnable)
            {
                if (m_maxVerticalAngle < m_minVertialAngle)
                    m_maxVerticalAngle = m_minVertialAngle;
            }
        }

        // Update is called once per frame
        void Update()
        {
			if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
			{
				ProcessTouchAction();
			}
			else
			{
				ProcessMouseAction();
			}

            CameraUpdate();
        }

        void LateUpdate() 
        {
            //CameraUpdate();
        }

        public override void CameraUpdate()
        {
			base.CameraUpdate();

            if (!m_isUpdate)
                return;

            if (m_followObject == null)
            {
                FlyControl();
            }
            else
            {
                switch (m_status)
                {
                    case FollowCameraStatus.Free:
                        {
                            break;
                        }

                    case FollowCameraStatus.Follow:
                        {
                            FollowControl();
                            break;
                        }

                    case FollowCameraStatus.Auto_Around_Target:
                        {
                            if (m_isAutoStop && m_enableStop)
                            {
                                m_currentStopTime += Time.deltaTime;
                                if (m_currentStopTime < m_autoStopTime)
                                {
                                    FollowControl();
									if (m_mouseAction && (Input.GetMouseButton(0) || Input.touchCount != 0))
                                        m_currentStopTime = 0.0f;
                                }
                                else
                                {
                                    m_isAutoStop = false;
                                }
                            }
                            else
                            {
                                if (m_AutoRotateInverse)
                                    transform.RotateAround(m_followObject.transform.position, Vector3.up, -m_autoRotateSpeed * Time.deltaTime);
                                else
                                    transform.RotateAround(m_followObject.transform.position, Vector3.up, m_autoRotateSpeed * Time.deltaTime);


                                if (m_mouseAction && (Input.GetMouseButton(0) || Input.touchCount != 0))
                                {
                                    m_isAutoStop = true;
                                    m_currentStopTime = 0.0f;
                                }
                            }
                            break;
                        }
                }
            }
        }

        public void SetFollowObj(GameObject obj)
        {
            m_followObject = obj;
            m_objBound = m_followObject.GetComponent<Renderer>().bounds;
        }

        public void SetFollowObj(GameObject obj, Vector3 position, Vector3 dirction)
        {

        }

        public void OnSwitchCamFinished()
        {

        }

        //Follow camera control stuffs.
        public void FollowControl()
        {
            if (m_followObject == null)
                return;

            if (m_mouseAction)
            {
                //Check platform for input
                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    //  one finger		
                    if (Input.touchCount == 1)
                    {
                        if (Input.GetTouch(0).phase == TouchPhase.Moved)
                        {
                            //					float yOffset = -Input.GetAxis("Mouse Y") * m_camRotateSpeed * m_iosRotateScale * Time.deltaTime;
                            //					float xOffset = Input.GetAxis("Mouse X") * m_camRotateSpeed * m_iosRotateScale * Time.deltaTime;
                            //					
                            //					m_rotSpeed.x = xOffset;
                            //					m_rotSpeed.y = yOffset;
                            //					//m_offset_value = m_follow_obj.transform.position - transform.position;

                            m_rotSpeed.x = Input.touches[0].deltaPosition.x;
                            m_rotSpeed.y = -Input.touches[0].deltaPosition.y;
                        }

                        m_currentStopTime = 0;
                    }
                    else if (Input.touchCount == 2) // more finger
                    {
                        Touch touch1 = Input.GetTouch(0);
                        Touch touch2 = Input.GetTouch(1);

                        if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
                        {
                            m_currentTouchDistance = Vector2.Distance(touch1.position, touch2.position);
                            m_lastTouchDistance = Vector2.Distance((touch1.position - touch1.deltaPosition), (touch2.position - touch2.deltaPosition));
                            //Move Horizontal
                            m_touchDeltaDistance = Mathf.Abs(m_currentTouchDistance - m_lastTouchDistance);
                            if (m_touchDeltaDistance < 0.5f)
                            {
                                //						m_tranSpeed.y = -Input.touches[1].deltaPosition.y;
                                //						m_tranSpeed.z = Input.touches[1].deltaPosition.x;
                            }
                            //Zoom stuffs.
                            else
                            {
                                m_tranSpeed.x = m_lastTouchDistance - m_currentTouchDistance;
                            }
                        }

                        m_currentStopTime = 0;
                    }
                    if (Input.touchCount == 3)
                    {
                        //Reset
                        //ResetCameraOrg("");
                        //				if (W_Main.g_obj.m_scene.m_banka_out)
                        //				{
                        //					W_Main.g_obj.m_main_camera.ResetLastLookAt();
                        //				}
                        //				else
                        //				{
                        //					W_Main.g_obj.m_main_camera.ResetCameraOrg("OnSetOrgCameraFinish");
                        //				}

                        m_currentStopTime = 0;
                    }
                    if (Input.touchCount == 4)
                    {

                        m_currentStopTime = 0;
                    }
                }
                else
                {
                    if (Input.GetMouseButton(0))
                    {
                        //                     float yOffset = -Input.GetAxis("Mouse Y") * m_camRotateSpeed * m_pcRotateScale * Time.deltaTime;
                        //                     float xOffset = Input.GetAxis("Mouse X") * m_camRotateSpeed * m_pcRotateScale * Time.deltaTime;
                        //                     //transform.RotateAround(m_followObject.transform.position, transform.right, yOffset);
                        //                     //transform.RotateAround(m_followObject.transform.position, Vector3.up, xOffset);
                        //                     //m_offset_value = m_follow_obj.transform.position - transform.position;
                        //                     				
                        //                     m_rotSpeed.x = xOffset;
                        //                     m_rotSpeed.y = yOffset;

                        m_rotSpeed.x = -Input.GetAxis("Mouse X");
                        m_rotSpeed.y = -Input.GetAxis("Mouse Y");

                        m_currentStopTime = 0;
                    }

                    //Move Horizontal
                    //			if(Input.GetMouseButton(2))
                    //			{
                    //				m_tranSpeed.y = -Input.GetAxis("Mouse Y");
                    //				m_tranSpeed.z = Input.GetAxis("Mouse X");
                    //				
                    //				m_currentStopTime = 0;
                    //			}

                    //Zoom in or out
                    if (Input.GetAxis("Mouse ScrollWheel") != 0)
                    {
                        m_tranSpeed.x = Input.GetAxis("Mouse ScrollWheel");

                        m_currentStopTime = 0;
                    }
                }
            }
            UpdateControlMovement();
            UpdateControlRotation();
            UpdateFollowedMovement();
        }

        //Check movement status including smooth or not. And then give a movement speed as a vector
        public void UpdateControlMovement()
        {
            //Check forward
            if (m_tranSpeed.x != 0)
            {
                Vector3 forwardVector = new Vector3(0, 0, 0);

                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    forwardVector = transform.forward * m_tranSpeed.x * m_camForwardSpeed * m_iosSpeedScale * Time.deltaTime;
                }
                else
                {
                    forwardVector = transform.forward * m_tranSpeed.x * m_camForwardSpeed * m_pcSpeedScale * Time.deltaTime;
                }

                //Check if smooth move
                if (m_isSmooth)
                {
                    if (m_tranSpeed.x > 0)
                    {
                        m_tranSpeed.x -= m_moveAccelarater;
                        if (m_tranSpeed.x < 0)
                            m_tranSpeed.x = 0;
                    }
                    else if (m_tranSpeed.x < 0)
                    {
                        m_tranSpeed.x += m_moveAccelarater;

                        if (m_tranSpeed.x > 0)
                            m_tranSpeed.x = 0;
                    }


                }
                else
                {
                    m_tranSpeed.x = 0;
                }

                //Update movement by vector.
                UpdateMovementByVector(forwardVector);
            }

            //Check horizontal movement
            if (m_tranSpeed.y != 0)
            {
                Vector3 upVector = new Vector3(0, 0, 0);
                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    upVector = Vector3.up * m_tranSpeed.y * m_camHoriSpeed * m_iosSpeedScale * Time.deltaTime;
                }
                else
                {
                    upVector = Vector3.up * m_tranSpeed.y * m_camHoriSpeed * m_pcSpeedScale * Time.deltaTime;
                }
                //Check if smooth move
                if (m_isSmooth)
                {
                    if (m_tranSpeed.y > 0)
                    {
                        m_tranSpeed.y -= m_moveAccelarater;
                        if (m_tranSpeed.y < 0)
                            m_tranSpeed.y = 0;
                    }
                    else if (m_tranSpeed.y < 0)
                    {
                        m_tranSpeed.y += m_moveAccelarater;

                        if (m_tranSpeed.y > 0)
                            m_tranSpeed.y = 0;
                    }
                }
                else
                {
                    m_tranSpeed.y = 0;
                }

                //Update movement by vector.
                UpdateMovementByVector(upVector);
            }

            //Check horizontal movement
            if (m_tranSpeed.z != 0)
            {
                Vector3 leftVector = new Vector3(0, 0, 0);

                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    leftVector = Vector3.left * m_tranSpeed.z * m_camHoriSpeed * m_iosSpeedScale * Time.deltaTime;
                }
                else
                {
                    leftVector = Vector3.left * m_tranSpeed.z * m_camHoriSpeed * m_pcSpeedScale * Time.deltaTime;
                }
                //Check if smooth move
                if (m_isSmooth)
                {
                    if (m_tranSpeed.z > 0)
                    {
                        m_tranSpeed.z -= m_moveAccelarater;
                        if (m_tranSpeed.z < 0)
                            m_tranSpeed.z = 0;
                    }
                    else if (m_tranSpeed.z < 0)
                    {
                        m_tranSpeed.z += m_moveAccelarater;

                        if (m_tranSpeed.z > 0)
                            m_tranSpeed.z = 0;
                    }
                }
                else
                {
                    m_tranSpeed.z = 0;
                }

                //Update movement by vector.
                UpdateMovementByVector(leftVector);
            }

        }

        public void UpdateFollowedMovement()
        {
            if (m_followObject == null)
                return;

            Vector3 diffTran = m_followObjLastPos - m_followObject.transform.position;
            Vector3 diffRot = m_followObjLastRot - m_followObject.transform.rotation.eulerAngles;
            UpdateMovementByVector(diffTran);
            UpdateRotateAngle(diffRot);
            m_followObjLastPos = m_followObject.transform.position;
            m_followObjLastRot = m_followObject.transform.rotation.eulerAngles;
        }

        //Check Movement Vector with movement limits and move camera.
        void UpdateMovementByVector(Vector3 moveVector)
        {
            Vector3 poseUpdated = transform.position - moveVector;
            float distanceUpdated = Vector3.Distance(m_followObject.transform.position, poseUpdated);
            m_currentDistance = Vector3.Distance(m_followObject.transform.position, transform.position);

            //Check limits to update camera position and rotation
            if (m_minDistanceEnable && m_maxDistanceEnable)
            {
                if (distanceUpdated > m_minDistance && distanceUpdated < m_maxDistance)
                {
                    if (m_currentDistance > moveVector.magnitude)
                        transform.position = poseUpdated;
                }
            }
            else if (!m_minDistanceEnable && m_maxDistanceEnable)
            {
                if (distanceUpdated < m_maxDistance)
                {
                    transform.position = poseUpdated;
                }
            }
            else if (m_minDistanceEnable && !m_maxDistanceEnable)
            {
                if (distanceUpdated > m_minDistance)
                {
                    if (m_currentDistance > moveVector.magnitude)
                        transform.position = poseUpdated;
                }
            }
            else
            {
                transform.position = poseUpdated;
            }
        }

        //Update rotate according status including smooth or not. And then give an rotate speed as a vector3.
        public void UpdateControlRotation()
        {
            Vector3 rotate = new Vector3(0, 0, 0);
            if (m_rotSpeed.x != 0)
            {
                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    rotate.y = m_rotSpeed.x * m_camRotateSpeed * m_iosRotateScale * Time.deltaTime;
                    //transform.RotateAround(m_followObject.transform.position, Vector3.up, m_rotSpeed.x * m_camRotateSpeed * m_iosRotateScale * Time.deltaTime);
                }
                else
                {
                    rotate.y = m_rotSpeed.x * m_camRotateSpeed * m_pcRotateScale * Time.deltaTime;
                    //transform.RotateAround(m_followObject.transform.position, Vector3.up, m_rotSpeed.x * m_camRotateSpeed * m_pcRotateScale * Time.deltaTime);
                }

                if (m_isSmooth)
                {
                    if (m_rotSpeed.x > 0)
                    {
                        m_rotSpeed.x -= m_rotateAccelarater;
                        if (m_rotSpeed.x < 0)
                            m_rotSpeed.x = 0;
                    }
                    else if (m_rotSpeed.x < 0)
                    {
                        m_rotSpeed.x += m_rotateAccelarater;
                        if (m_rotSpeed.x > 0)
                            m_rotSpeed.x = 0;
                    }
                }
                else
                {
                    m_rotSpeed.x = 0;
                }
            }

            if (m_rotSpeed.y != 0)
            {
                Vector3 originVector3 = new Vector3(0, 1, 0);
                Vector3 currentVector = Vector3.Normalize(transform.position - m_followObject.transform.position);
                m_verticalAngle = 90.0f - Vector3.Angle(originVector3, currentVector);
                float yRotateAngle = 0.0f;

                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    //rotate.x = m_rotSpeed.y * m_iosRotateScale * Time.deltaTime;
                    yRotateAngle = m_rotSpeed.y * m_iosRotateScale * Time.deltaTime + m_verticalAngle;
                }
                else
                {
                    //rotate.x = m_rotSpeed.y * m_pcRotateScale * Time.deltaTime;
                    yRotateAngle = m_rotSpeed.y * m_pcRotateScale * Time.deltaTime + m_verticalAngle;
                }

                if (m_isSmooth)
                {
                    if (m_rotSpeed.y > 0)
                    {
                        m_rotSpeed.y -= m_rotateAccelarater;
                        if (m_rotSpeed.y < 0)
                            m_rotSpeed.y = 0;
                    }
                    else if (m_rotSpeed.y < 0)
                    {
                        m_rotSpeed.y += m_rotateAccelarater;
                        if (m_rotSpeed.y > 0)
                            m_rotSpeed.y = 0;
                    }
                }
                else
                {
                    m_rotSpeed.y = 0;
                }

                //Check the limitation of rotation angle
                if (m_minVertialEnable && m_maxVertialEnable)
                {
                    if (yRotateAngle < m_maxVerticalAngle && yRotateAngle > m_minVertialAngle)
                    {
                        rotate.x = yRotateAngle - m_verticalAngle;
                    }
                }
                else if (!m_minVertialEnable && m_maxVertialEnable)
                {
                    if (yRotateAngle < m_maxVerticalAngle)
                    {
                        rotate.x = yRotateAngle - m_verticalAngle;
                    }
                }
                else if (m_minVertialEnable && !m_maxVertialEnable)
                {
                    if (yRotateAngle > m_minVertialAngle)
                    {
                        rotate.x = yRotateAngle - m_verticalAngle;
                    }
                }
                else
                {
                    rotate.x = yRotateAngle - m_verticalAngle;
                }
                UpdateRotateAngle(rotate);
            }
        }

        public void UpdateRotateAngle(Vector3 angle)
        {
            //Rotate x axis
            transform.RotateAround(m_followObject.transform.position, transform.right, angle.x);

            //Rotate y axis
            transform.RotateAround(m_followObject.transform.position, m_followObject.transform.up, -angle.y);

            //Rotate z axis
            transform.RotateAround(m_followObject.transform.position, -m_followObject.transform.forward, angle.z);
        }
    }
}
