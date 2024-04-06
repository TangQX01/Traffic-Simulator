using System;
using System.Collections.Generic;
using UnityEngine;
using SIP.FastVRTools.Cameras;
using SIP.Common;

namespace SIP.FastVRTools
{
    public enum CM_MSGID
    {
        SWITCH_CAM_BY_NUM,
        SWITCH_CAM_BY_NAME,
        NONE,
    }

    [AddComponentMenu("SIP/FastVRTool/Managers/Camera Manager")]
    [Serializable]
    public class FVRCameraManager : FVRManagerBase
    {
        // Base camera info [3/27/2014 leo]
        [HideInInspector]
        public List<Camera> m_cameras = new List<Camera>();             // Camera List [3/27/2014 leo]
        public Camera m_startCamera;                                  // Start Camera [3/27/2014 leo]
        [HideInInspector]
        public int m_camTotalNum;                                       //Total camera numbers [3/27/2014 leo]
        [HideInInspector]
        public int m_currentCamNum;                                     //Current camera number [3/27/2014 leo]
        [HideInInspector]
        public int m_nextCamNum;                                        //Next Change Camera number [3/27/2014 leo]
		[HideInInspector]
		public int m_lastCamNum;										//Last camera number

		public static Camera s_currentCam;								//Add Current Camera by leo [09/13/2014 leo]

        //Switch Camera stuffs. [3/27/2014 leo]
        [HideInInspector]
        public bool m_isSwitchFinished = true;			                //Check if switch is finished.
        public bool m_enableMaualSwitch = true;			                //Switch camera by pressing ">" or "<" key.
        public float m_switchTime = 1.0f;				                //Switch camera time.

        //Auto Play Switch Time [3/27/2014 leo]
        public List<Camera> m_autoSwitchCameras = new List<Camera>();   //Auto Switch Cameras
        int m_currentAutoSwitchCamera = 0;				                //Current Active Switch Camera;
        public float m_AutoPlaySwitchTime = 5.0f;		                //Auto play Switch Time;
        float m_currentPlayTime = 0.0f;					                //Current Auto Play Time;
        [HideInInspector]
        public bool m_isAutoPause = false;				                //Is Auto Play Pause;
        public float m_PauseTime = 5.0f;
        public bool m_DisableOtherCam = true;                           //Is Auto Disable other cameras at start.
        float m_currentPauseTime = 0.0f;

        void Awake()
        {
            base.Awake();
            // Get camera numbers [4/3/2014 leo]
            m_camTotalNum = transform.childCount;
            if (m_camTotalNum == 0)
            {
                Debug.Log("Camera Manager: There is no cameras in the camera list!");
                return;
            }

            // Set active camera [4/3/2014 leo]
			if (m_startCamera == null)
			{
                m_startCamera = transform.GetChild(0).GetComponent<Camera>();

				//[09/13/2014 leo] Set current camera;
				s_currentCam = m_startCamera;
			}

            // Add all cameras to camera lists [4/3/2014 leo]
            for (int i = 0; i < m_camTotalNum; i++)
            {
                Camera tempCam = transform.GetChild(i).GetComponent<Camera>();
                m_cameras.Add(tempCam);
                if(m_DisableOtherCam == true)
                {
                    tempCam.gameObject.SetActive(false);

                    if (m_startCamera.name == tempCam.name)
                    {
                        tempCam.gameObject.SetActive(true);
                        m_currentCamNum = i;
                        m_lastCamNum = i;
                    }
                }
            }
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (m_enableMaualSwitch)
            {
                KeyboardControl();
            }
        }

        public override void SystemMsg(MSG_TYPES msgType, object msgId, object param)
        {
            if (msgType == MSG_TYPES.CameraManager)
            {
                if ((CM_MSGID)msgId == CM_MSGID.SWITCH_CAM_BY_NUM)
                {
                    SwithCameraByNum((int)param);
                }
                else if ((CM_MSGID)msgId == CM_MSGID.SWITCH_CAM_BY_NAME)
                {
                    SwitchCameraByName((string)param);
                }
            }
        }

        void KeyboardControl()
        {
            int swithCamNum = 0;
            if (Input.GetKeyDown(KeyCode.Comma))
            {
                swithCamNum = m_currentCamNum - 1;
                if (swithCamNum < 0)
                    swithCamNum = m_camTotalNum - 1;

                SwithCameraByNum(swithCamNum);
            }

            if (Input.GetKeyDown(KeyCode.Period))
            {
                swithCamNum = m_currentCamNum + 1;
                if (swithCamNum >= m_camTotalNum)
                    swithCamNum = 0;

                SwithCameraByNum(swithCamNum);
            }
        }

        public void SwitchCameraByName(string name)
        {
            for (int i = 0; i < m_cameras.Count; i++)
            {
                if (m_cameras[i].name == name)
                {
                    SwithCameraByNum(i);
                    return;
                }
            }
        }

		public void SwitchCameraByName(string name, bool isSmooth)
		{
			for (int i = 0; i < m_cameras.Count; i++)
			{
				if (m_cameras[i].name == name)
				{
					SwithCameraByNum(i, isSmooth);
					return;
				}
			}
		}
		
        public void SwithCameraByNum(int number)
        {
            if (!m_isSwitchFinished)
                return;
            else
                m_isSwitchFinished = false;

			//Get Last Camera number;
			m_lastCamNum = m_currentCamNum;

            //Get Next Camera number
            m_nextCamNum = number;

            // Clone an switch camera [4/3/2014 leo]
            Camera switchCamera = (Camera)Instantiate(m_cameras[m_currentCamNum]);
            switchCamera.name = "switchCamera";

            CameraBase curCamBase = m_cameras[m_currentCamNum].GetComponent<CameraBase>();
            if(curCamBase != null)
                curCamBase.ResetPose();

            //Add Camera to camera list.
            m_cameras.Add(switchCamera.GetComponent<Camera>());

            //Disable current camera and active next camera.
            m_cameras[m_currentCamNum].gameObject.SetActive(false);
            m_currentCamNum = m_cameras.Count - 1;
            m_cameras[m_currentCamNum].gameObject.SetActive(true);

			//[09/13/2014 leo] update current camera;
			s_currentCam = switchCamera;

            //Set Camera Properties
            //switchCamera.camera.cullingMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Water"));
            

            //Move Camera to next camera.
            iTween.MoveTo(m_cameras[m_currentCamNum].gameObject, iTween.Hash("position", m_cameras[m_nextCamNum].transform.position,
                "time", m_switchTime, "onComplete", "FinishSwitchCam", "onCompleteTarget", gameObject));
            iTween.RotateTo(m_cameras[m_currentCamNum].gameObject, m_cameras[m_nextCamNum].transform.rotation.eulerAngles,
                m_switchTime);
        }

		public void SwithCameraByNum(int number, bool isSmooth)
		{
			if(isSmooth)
			{
				SwithCameraByNum(number);
			}
			else
			{
				//Get Next Camera number
				m_nextCamNum = number;

				//Disable current camera.
				m_cameras[m_currentCamNum].gameObject.SetActive(false);
				m_cameras[m_nextCamNum].gameObject.SetActive(true);
				m_currentCamNum = m_nextCamNum;
			}
		}

		public void SwitchToLastCamera()
		{
			SwithCameraByNum(m_lastCamNum);
		}

		public void SwitchToLastCamera(bool isSmooth)
		{
			SwithCameraByNum(m_lastCamNum, isSmooth);
		}

        public void FinishSwitchCam()
        {
//             FollowCamera nextCam = m_cameras[m_nextCamNum].GetComponent<FollowCamera>();
//             nextCam.m_isAutoStop = false;
            m_cameras[m_currentCamNum].gameObject.SetActive(false);
            m_cameras[m_nextCamNum].gameObject.SetActive(true);

			//[09/13/2014 leo] update current camera;
			s_currentCam = m_cameras[m_nextCamNum];
            Destroy(m_cameras[m_currentCamNum].gameObject);
            m_cameras.RemoveAt(m_currentCamNum);
            m_currentCamNum = m_nextCamNum;
            m_isSwitchFinished = true;
        }

        public void SwitchCameraByCamera(Camera cam)
        {

        }

        public void AutoSwitchCamera()
        {

        }

        public void AutoPause(bool pause)
        {
            m_isAutoPause = pause;
            m_currentPauseTime = 0.0f;
        }

        public CameraBase GetCamById(int id)
        {
            return m_cameras[id].GetComponent<CameraBase>();        
        }
    }
}
