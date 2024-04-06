using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;
using SIP.FastVRTools;
using SIP.FastVRTools.Cameras;

namespace CitySim {
    partial class CarManager : MonoBehaviour
    {
        /// <summary>
        /// Leo: Get Car prefabs.
        /// </summary>
        public GameObject[] m_Cars;

        /// <summary>
        /// Leo: Get different way points.
        /// </summary>
        public Transform m_Way;

        /// <summary>
        /// Leo: Get Camera managers.
        /// </summary>
        public FVRCameraManager m_CameraManager;

        public int m_MaxCarNum = 20;

        private int m_CarNum = 0;

        public float m_RefreshCarTime = 10.0f;

        public float m_TimePass = 0.0f;

        //[HideInInspector]
        public List<FollowCamera> m_CamList = new List<FollowCamera>();
        [HideInInspector]
        public SocketTest m_Socket = new SocketTest();

        private List<string> m_CamFollowObjName = new List<string>();


        private void Start()
        {

            for (int i = 0; i < m_Way.childCount; i++)
            {
                GameObject wayPoint = m_Way.GetChild(i).gameObject;
                WaypointCircuit wayCircuit = wayPoint.GetComponent<WaypointCircuit>();
                CreateCarByWay(wayCircuit);
            }

            for (int i = 0; i < m_CameraManager.m_cameras.Count; i++)
            {
                var cam = m_CameraManager.GetCamById(i);
                if (cam)
                {
                    if (cam.GetType() == typeof(FollowCamera))
                    {
                        m_CamList.Add((FollowCamera)cam);

                        /// <summary>
                        /// Tqx: ��ʼ��ͼ��Э��
                        /// </summary>
                        //var len = m_CamList.Count;
                        //print(m_CamList[len-1].gameObject.name);
                        //print(m_CamList[len - 1].GetComponent<Camera>().GetType());
                        //print(m_CamList[i].name);
                        //StartCoroutine(WaitHOIDetection(m_CamList[m_CamList.Count - 1].GetComponent<Camera>()));
                    }
                }
            }
            /// <summary>
            /// Tqx: m_TimePass needs to be initialized.
            /// </summary>
            m_TimePass = 0.0f;
        }

        // Update is called once per frame
        void Update()
        {
            UpdateCarNum();
            UpdateCameras();
        }

        void CreateCarByWay(WaypointCircuit wayCircuit)
        {
            //print(m_CarNum);
            Vector3 dir = wayCircuit.Waypoints[1].position - wayCircuit.Waypoints[0].position;
            Quaternion rot = Quaternion.LookRotation(dir);
            GameObject obj = Instantiate(m_Cars[Random.Range(0, m_Cars.Length - 1)], wayCircuit.Waypoints[0].position, rot);
            obj.name = "Car " + (m_CarNum++).ToString("0000");
            obj.transform.parent = transform;
            obj.GetComponent<WaypointProgressTracker>().circuit = wayCircuit;
            obj.GetComponent<WaypointProgressTracker>().myStart();
        }

        void UpdateCarNum()
        {
            m_TimePass += Time.deltaTime;
            if (m_TimePass < m_RefreshCarTime)
                return;

            m_TimePass = 0.0f;

            if (transform.childCount < m_MaxCarNum)
            {
                int wayCount = m_Way.childCount;
                int wayId = Random.Range(0, wayCount);
                GameObject wayPoint = m_Way.GetChild(wayId).gameObject;
                WaypointCircuit wayCircuit = wayPoint.GetComponent<WaypointCircuit>();
                CreateCarByWay(wayCircuit);
            }
        }

        void UpdateCameras()
        {
            if (m_CamList.Count == 0)
                return;

            
            //m_CamFollowObjName.Clear();

            for (int i = 0; i < m_CamList.Count; i++)
            {
                FollowCamera cam = m_CamList[i];
                if (cam.m_followObject == null)
                {
                    GameObject followObj = null;

                    for (int j = 0; j < transform.childCount; j++)
                    {
                        followObj = transform.GetChild(j).gameObject;

                        for (int k = 0; k < m_CamFollowObjName.Count; k++)
                        {
                            if (followObj.name == m_CamFollowObjName[k])
                            {
                                followObj = null;
                                break;
                            }
                        }

                        if (followObj != null)
                            break;
                    }

                    if (followObj != null)
                    {
                        cam.m_followObject = followObj;
                        cam.transform.position = followObj.transform.Find("CameraFocusPoint").position;
                        cam.transform.rotation = followObj.transform.Find("CameraFocusPoint").rotation;
                        m_CamFollowObjName.Add(followObj.name);
                    }
                }
                cam.transform.position = cam.m_followObject.transform.Find("CameraFocusPoint").position;
                cam.transform.rotation = cam.m_followObject.transform.Find("CameraFocusPoint").rotation;
            }
        }
    }
}