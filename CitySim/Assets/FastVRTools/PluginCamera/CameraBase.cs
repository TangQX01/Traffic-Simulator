using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIP.Common;
using UnityEngine;
using SIP.FastVRTools.MultiTouch;

namespace SIP.FastVRTools.Cameras
{
    public enum CamType
    {
        Fly = 1,		//Fly Camera
        Follow,			//Object Following Camera
        Fix,			//Fix point camera
        FirstPerson,	//First Person Camera
        Animate			//Animation camera
    }

    public enum ControlType
    {
        Mouse,
        TouchInput,
        JoyStick,
    }

    public class MouseOrTouch
    {
        public Vector2 pos;				// Current position of the mouse or touch event
        public Vector2 lastPos;			// Previous position of the mouse or touch event
        public Vector2 delta;			// Delta since last update
        public Vector2 totalDelta;		// Delta since the event started being tracked

        public Camera pressedCam;		// Camera that the OnPress(true) was fired with

        public GameObject last;			// Last object under the touch or mouse
        public GameObject current;		// Current game object under the touch or mouse
        public GameObject pressed;		// Last game object to receive OnPress
        public GameObject dragged;		// Game object that's being dragged

        public float clickTime = 0f;	// The last time a click event was sent out

        public bool touchBegan = true;
        public bool pressStarted = false;
        public bool dragStarted = false;

        public ControlType controlType;

        public MouseOrTouch()
        {

        }

        public MouseOrTouch(ControlType type)
        {
            controlType = type;
        }
    }

    [AddComponentMenu("SIP/FastVRTool/Cameras/Base Camera")]

    public abstract class CameraBase : MonoBehaviour
    {
        //Camera Speeds
        public bool m_isLockCamSpeed = false;		//if the camera speed is locked, it won't change during the game.
        public float m_camForwardSpeed = 1.0f;      //Camera Move Forward or BackWard Speed
        public float m_camHoriSpeed = 1.0f;			//Camera Move Left Right or Up Down Speed
        public float m_camRotateSpeed = 1.0f;		//Camera Rotate Speed. If it's a fly camera this is
        //Self rotate speed, if it's an object surrond camera,
        //this is surrond rotate speed.
        //Camera Property
        public CamType m_camType;					//Camera Type
        //public bool m_isCurrentCam;				//Check if this is current camera.

        /// <summary>
        /// Active mouse click action or not!
        /// </summary>
        public bool m_mouseAction = true;

        /// <summary>
        /// The cameras without mouse action. eg: UI Cameras
        /// </summary>
        public List<Camera> m_noActionCamera = new List<Camera>();

        [HideInInspector]
        public Vector3 m_camPosOrigin;				//Camera Origin Position
        [HideInInspector]
        public Quaternion m_camQuatOrigin;			//Camera Origin Quaternion

        //GUI Showing Parameters
        float m_elapsedTime = 0.0f;					//Show time elapsed.
        bool m_isShowButton = true;				//If using iphone, we want to show buttons to change camera speed.
        float m_showTime = 5.0f;				//Show Camera parameter time.

        #region Mouse Or Touch Properties
        /// <summary>
        /// Whether the touch-based input is used.
        /// </summary>
        public bool m_isTouch = false;

        /// <summary>
        /// Whether the touch-based input is used.
        /// </summary>
        public bool m_isMouse = true;

		/// <summary>
		/// Whether the TUIO Touch input is used.
		/// </summary>
		public bool m_isTuioTouch = false;

        /// <summary>
        /// 4 Touches are support by default. 
        /// </summary>
		public MouseOrTouch[] m_touches = new MouseOrTouch[] { new MouseOrTouch(ControlType.TouchInput), new MouseOrTouch(ControlType.TouchInput), new MouseOrTouch(ControlType.TouchInput), new MouseOrTouch(ControlType.TouchInput) };

		/// <summary>
		/// This is current touch
		/// </summary>
		public static MouseOrTouch m_currentTouch = null;

		/// <summary>
		/// Dictionary for all active objects.
		/// </summary>
		public static Dictionary<int, MouseOrTouch> m_activeTouches = new Dictionary<int, MouseOrTouch>();

		public static RaycastHit m_hit;

        /// <summary>
        /// Current Mouse
        /// </summary>
        public MouseOrTouch m_mouse = new MouseOrTouch(ControlType.Mouse);

        /// <summary>
        /// For touch screen, this is the last touch position.
        /// For the mouse, this is the mouse current scene screen pos.
        /// </summary>
        public Vector2 m_lastTouchPos = new Vector2();

        /// <summary>
        /// Set to 'true' just before OnDrag-related events are sent. No longer needed, but kept for backwards compatibility.
        /// </summary>
        public bool m_isDragging = true;

        /// <summary>
        /// How much the mouse has to be moved after pressing a button before it starts to send out drag events.
        /// </summary>
        public float m_mouseDragThreshold = 4f;

        /// <summary>
        /// How much the mouse has to be moved after pressing a button before it starts to send out drag events.
        /// </summary>
        public float m_touchDragThreshold = 40f;

        /// <summary>
        /// How far the mouse is allowed to move in pixels before it's no longer considered for click events, if the click notification is based on delta.
        /// </summary>
        public float m_mouseClickThreshold = 10f;

        /// <summary>
        /// How far the touch is allowed to move in pixels before it's no longer considered for click events, if the click notification is based on delta.
        /// </summary>
        public float m_touchClickThreshold = 40f;

        /// <summary>
        /// The last hover object.
        /// </summary>
        public GameObject m_objHover;

        /// <summary>
        /// This is the time elpased for the next ray cast.
        /// </summary>
        public float m_nextRayCastTime = 0.0f;

        #endregion

        //==================================== Unity Functions [2/28/2014 leo] ====================================//
        public virtual void Start()
        {
            Init();
        }

        public virtual void Awake()
        {

        }

        public virtual void OnGUI()
        {
            UpdateGUI();
        }

        void Update()
        {

        }

        //=========================================================================================================//


        //===================================== Base Functions [2/28/2014 leo] ====================================//

//         public override void SystemMsg(string msg)
//         {
// 
//         }

        //=========================================================================================================//


        //==================================== Custom Functions [2/28/2014 leo] ===================================//
        public void Init()
        {
            m_camPosOrigin = transform.position;
            m_camQuatOrigin = transform.rotation;

            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
            {
                m_isMouse = false;
                m_isTouch = true;
            }
            else
            {
                m_isMouse = true;
                m_isTouch = false;
            }
        }

        public void ResetPose()
        {
            transform.position = m_camPosOrigin;
            transform.rotation = m_camQuatOrigin;
        }

        //Move to custom transform
        public void MoveTo(Vector3 pos, Vector3 rot, float time)
        {
            iTween.MoveTo(this.gameObject, pos, time);
            iTween.RotateTo(this.gameObject, rot, time);
        }

        //Move to target transform
        public void MoveToTarget(GameObject go, float time)
        {
            if (go == null)
                return;

            Vector3 pos = go.transform.position;
            Vector3 rot = go.transform.rotation.eulerAngles;
            MoveTo(pos, rot, time);

        }

        //Get Camera position.
        public Vector3 GetPos()
        {
            return this.gameObject.transform.position;
        }

        public void OnChangeSpeed()
        {
            //Check if the camera speed is lock
            if (m_isLockCamSpeed)
                return;

            //Minus Rotate Speed
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.Minus))
            {
                AddRotateSpeed(-1.0f);

                //Show Texts
                m_elapsedTime = m_showTime;
            }
            //Plus Rotate Speed
            else if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.Equals))
            {
                AddRotateSpeed(1.0f);

                //Show Texts
                m_elapsedTime = m_showTime;
            }
            //Minus Horizontal Speed
            else if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKey(KeyCode.Minus))
            {
                AddHoriSpeed(-1.0f);

                //Show Texts
                m_elapsedTime = m_showTime;
            }
            //Plus Horizontal Speed
            else if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKey(KeyCode.Equals))
            {
                AddHoriSpeed(1.0f);

                //Show Texts
                m_elapsedTime = m_showTime;
            }
            //Minus Forward Speed
            else if (Input.GetKey(KeyCode.Minus))
            {
                AddForwardSpeed(-1.0f);

                //Show Texts
                m_elapsedTime = m_showTime;
            }
            //Plus Forward Speed
            else if (Input.GetKey(KeyCode.Equals))
            {
                AddForwardSpeed(1.0f);

                //Show Texts
                m_elapsedTime = m_showTime;
            }

            if (Input.GetKey(KeyCode.R))
            {
                MoveTo(m_camPosOrigin, m_camQuatOrigin.eulerAngles, 1.0f);
            }


            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
            {
                if (Input.touchCount == 4)
                {
                    //Show Texts
                    m_elapsedTime = m_showTime;

                    //Show Buttons
                    m_isShowButton = true;
                }
            }

        }

        //Add or minus rotate speed.
        public void AddRotateSpeed(float speed)
        {
            m_camRotateSpeed += speed * Time.deltaTime;

            if (m_camRotateSpeed < 0.0f)
            {
                m_camRotateSpeed = 0.0f;
            }
        }

        //Add or minus horizontal speed.
        public void AddHoriSpeed(float speed)
        {
            m_camHoriSpeed += speed * Time.deltaTime;

            if (m_camHoriSpeed < 0.0f)
            {
                m_camHoriSpeed = 0.0f;
            }
        }

        //Add or minus forward speed.
        public void AddForwardSpeed(float speed)
        {
            m_camForwardSpeed += speed * Time.deltaTime;

            if (m_camForwardSpeed < 0.0f)
            {
                m_camForwardSpeed = 0.0f;
            }
        }

        //Speed GUI Update
        public void UpdateGUI()
        {
            if (m_elapsedTime > 0.0f)
            {
                m_elapsedTime -= Time.deltaTime;
                if (m_elapsedTime < 0.0f)
                {
                    m_elapsedTime = 0.0f;
                }
                Rect rect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 10, 400, 20);

                GUI.Label(rect, "forward speed: " + m_camForwardSpeed.ToString("F2") + " rotate speed: " +
                    m_camRotateSpeed.ToString("F2") + " Horizontal Speed: " + m_camHoriSpeed.ToString("F2"));


                if (m_isShowButton)
                {

                }
            }
        }

		public bool Raycast(Vector3 pos, out RaycastHit hit)
		{
			Ray ray = GetComponent<Camera>().ScreenPointToRay(pos);

			if(Physics.Raycast(ray, out hit, 3000))
			{
				m_objHover = hit.collider.gameObject;
				return true;
			}
			else
			{
				m_objHover = null;
				return false;
			}
		}

        public GameObject GetMouseObject()
        {
            GameObject objClick = null;
            RaycastHit hit;
#if UNITY_IPHONE || UNITY_ANDROID
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.touches[0].position);
#else
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
#endif

            if (Physics.Raycast(ray, out hit, 3000))
            {
                objClick = hit.collider.gameObject;
            }

            return objClick;
        }

        public bool isCurrentLayer()
        {
            GameObject obj = GetMouseObject();
            if (obj != null)
            {
                //Debug.Log("<Camera Base> game object Name:" + obj.name + " Layer: " + obj.layer);
                //Debug.Log("<Camera Base> Camera game object Name:" + gameObject.name + " Layer: " + gameObject.layer);
                if (obj.layer != gameObject.layer)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        // Virtual Overide Functions [3/4/2014 leo]
        public virtual void CameraUpdate()
        {
			CheckNoActionCamera();
			OnChangeSpeed();
        }

        //Get Fake Touch Event To Fit Touch Screen.
        public void ProcessFakeTouch()
        {
            if (!m_isTouch)
                return;

            bool pressed = Input.GetMouseButtonDown(0);
            bool unpressed = Input.GetMouseButtonUp(0);
            bool held = Input.GetMouseButton(0);
        }

        public void CheckNoActionCamera()
        {
            for (int i = 0; i < m_noActionCamera.Count; i++ )
            {
                GameObject obj = null;
                RaycastHit hit = new RaycastHit();

#if UNITY_IPHONE || UNITY_ANDROID
                Ray ray = m_noActionCamera[i].ScreenPointToRay(Input.touches[0].position);
#else
                Ray ray = m_noActionCamera[i].ScreenPointToRay(Input.mousePosition);
#endif
                if (Physics.Raycast(ray, out hit, 100))
                {
                    obj = hit.collider.gameObject;
                }

                if (obj != null)
                {
                    if (((1 << obj.layer) & m_noActionCamera[i].cullingMask) != 0)
                    {
                        m_mouseAction = false;
                    }
                    else
                    {
                        m_mouseAction = true;
                    }
                }
                else
                {
#if UNITY_IPHONE || UNITY_ANDROID
                    if (Input.touchCount == 0)
                    {
                        m_mouseAction = true;
                    }
#else
                    if (!Input.GetMouseButton(0))
                    {
                        m_mouseAction = true;
                    }
#endif
                }
            }
        }

        public void ProcessMouseAction()
        {
            if (!m_isMouse)
                return;

            // Update the position and delta
            m_lastTouchPos = Input.mousePosition;
            m_mouse.delta = m_lastTouchPos - m_mouse.pos;
            m_mouse.pos = m_lastTouchPos;
            bool posChanged = m_mouse.delta.sqrMagnitude > 0.001f;

            // Is any button currently pressed?
            bool isPressed = false;
            bool justPressed = false;
            if (Input.GetMouseButtonDown(0))
            {
                justPressed = true;
                isPressed = true;
            }
            else if (Input.GetMouseButton(0))
            {
                isPressed = true;
            }

            bool isUnPressed = Input.GetMouseButtonUp(0);

            // We don't neet to ray cast every frame. [6/8/2014 leo]
            if (isPressed || posChanged || m_nextRayCastTime < Time.time)
            {
                m_nextRayCastTime = Time.time + 0.02f;
                m_mouse.current = GetMouseObject();
            }

            bool highlightChanged = (m_mouse.last != m_mouse.current);

            // The button was released over a different object -- remove the highlight from the previous
            if ((justPressed || !isPressed) && highlightChanged)
            {
                Notify(m_objHover, "OnHover", false);
                m_objHover = null;
            }

            ProcessTouchOrMouse(m_mouse, isPressed, isUnPressed);

            // If nothing is pressed and there is an object under the touch, highlight it
            if (!isPressed && highlightChanged)
            {
                m_objHover = m_mouse.current;
                Notify(m_objHover, "OnHover", true);
            }

            // Update the last value
            m_mouse.last = m_mouse.current;
        }

        public void ProcessTouchAction()
        {
			for(int i = 0; i < Input.touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				m_currentTouch = GetTouch(i);

				bool pressed = (touch.phase == TouchPhase.Began) || m_currentTouch.touchBegan;
				bool unpressed = (touch.phase == TouchPhase.Canceled) || (touch.phase == TouchPhase.Ended);
				m_currentTouch.touchBegan = false;
				//Get Touch delta distance
				m_currentTouch.delta = pressed ? Vector2.zero : touch.position - m_currentTouch.pos;
				m_currentTouch.pos = touch.position;

				Raycast(m_currentTouch.pos, out m_hit);

				m_currentTouch.last = m_currentTouch.current;
				m_currentTouch.current = m_objHover;
				m_lastTouchPos = m_currentTouch.pos;

                if (pressed) m_currentTouch.pressedCam = GetComponent<Camera>();

				if(touch.tapCount > 1) m_currentTouch.clickTime = Time.time;

				ProcessTouchOrMouse(m_currentTouch, pressed, unpressed);

				//Remove 
				if(unpressed)
					RemoveTouch(i);
				m_currentTouch.last = null;
				m_currentTouch = null;
			}
        }

        public void ProcessTouchOrMouse(MouseOrTouch touch, bool pressed, bool unpressed)
        {
            if (touch == null)
                return;

            // Whether we're using the mouse
            bool isMouse = (touch.controlType == ControlType.Mouse);
            float drag = isMouse ? m_mouseDragThreshold : m_touchDragThreshold;
            float click = isMouse ? m_mouseClickThreshold : m_touchClickThreshold;

            if (pressed)
            {
                touch.pressStarted = true;
                Notify(touch.pressed, "OnPress", false);
                touch.pressed = touch.current;
                touch.dragged = touch.current;
                touch.totalDelta = Vector2.zero;
                touch.dragStarted = false;
                Notify(touch.pressed, "OnPress", true);
            }
            else if (touch.pressed != null && (touch.delta.magnitude != 0f || touch.current != touch.last))
            {
                // Keep track of the total movement [6/5/2014 leo]
                touch.totalDelta += touch.delta;
                float mag = touch.totalDelta.magnitude;
                bool justStarted = false;

                // If the drag process hasn't started yet but we've already moved off the object, start it immediately
                if (!touch.dragStarted && touch.last != touch.current)
                {
                    touch.dragStarted = true;
                    touch.delta = touch.totalDelta;

                    // OnDragOver is sent for consistency, so that OnDragOut is always preceded by OnDragOver
                    m_isDragging = true;
                    Notify(touch.dragged, "OnDragStart", null);
                    Notify(touch.last, "OnDragOver", touch.dragged);
                    m_isDragging = false;
                }
                else if (!touch.dragStarted && drag < mag)
                {
                    // If the drag event has not yet started, see if we've dragged the touch far enough to start it
                    justStarted = true;
                    touch.dragStarted = true;
                    touch.delta = touch.totalDelta;
                }

                // If we're dragging the touch, send out drag events
                if (touch.dragStarted)
                {
                    m_isDragging = true;

                    if (justStarted)
                    {
                        Notify(touch.dragged, "OnDragStart", null);
                        Notify(touch.current, "OnDragOver", touch.dragged);
                    }
                    else if (touch.last != touch.current)
                    {
                        Notify(touch.last, "OnDragOut", touch.dragged);
                        Notify(touch.current, "OnDragOver", touch.dragged);
                    }

                    Notify(touch.dragged, "OnDrag", touch.delta);

                    touch.last = touch.current;
                    m_isDragging = false;
                }
            }

            if (unpressed)
            {
                touch.pressStarted = false;

                if (touch.pressed != null)
                {
                    // If there was a drag event in progress, make sure OnDragOut gets sent
                    if (touch.dragStarted)
                    {
                        Notify(touch.last, "OnDragOut", touch.dragged);
                        Notify(touch.dragged, "OnDragEnd", null);
                    }

                    // Send the notification of a touch ending
                    Notify(touch.pressed, "OnPress", false);

                    // Send a hover message to the object
                    if (m_isMouse) Notify(touch.current, "OnHover", true);
                    m_objHover = touch.current;

                    // If the button/touch was released on the same object, consider it a click and select it
                    if (touch.dragged == touch.current || touch.totalDelta.magnitude < drag)
                    {
                        float time = Time.time;

                        Notify(touch.pressed, "OnClick", null);

                        if (touch.clickTime + 0.35f > time)
                        {
                            Notify(touch.pressed, "OnDoubleClick", null);
                        }
                        touch.clickTime = time;
                    }
                    else if (touch.dragStarted) // The button/touch was released on a different object
                    {
                        // Send a drop notification (for drag & drop)
                        Notify(touch.current, "OnDrop", touch.dragged);
                    }
                }
                touch.dragStarted = false;
                touch.pressed = null;
                touch.dragged = null;
            }
        }

        public void Notify(GameObject go, string funcName, object obj)
        {
            if (go != null)
            {
                go.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
            }
        }

		/// <summary>
		/// Add touch to active touch dictionary
		/// </summary>
		/// <returns>Return MouseOrTouch object.</returns>
		/// <param name="id">Identifier.</param>
		static public MouseOrTouch GetTouch(int id)
		{
			MouseOrTouch touch = null;

			if(id < 0)
				return null;

			if(!m_activeTouches.TryGetValue(id, out touch))
			{
				touch = new MouseOrTouch(ControlType.TouchInput);
				touch.touchBegan = true;
				m_activeTouches.Add(id, touch);
			}
			return touch;
		}

		/// <summary>
		/// Removes unpressed touch from active touch dictionary
		/// </summary>
		/// <param name="id">Identifier.</param>
		public static void RemoveTouch(int id)
		{
			m_activeTouches.Remove(id);
		}
    }
}
