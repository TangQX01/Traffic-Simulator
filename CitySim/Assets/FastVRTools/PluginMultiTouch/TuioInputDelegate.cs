using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SIP.FastVRTools.MultiTouch.TUIO;

namespace SIP.FastVRTools.MultiTouch
{
    public class TuioInputDelegate : MonoBehaviour
    {
        public float m_TUIOUpdateFrequency = 100.0f;
        public Dictionary<long, TuioTouchEvent> m_activeEvents = new Dictionary<long, TuioTouchEvent>(100);

        protected ArrayList m_eventQueue = new ArrayList();
        protected Object m_eventQueueLock = new Object();

        protected TuioInputController tuioInput;

        protected float cameraPixelWidth;
        protected float cameraPixelHeight;

        void Awake()
        {
            tuioInput = new TuioInputController();
            tuioInput.m_collectEvents = true;
            cameraPixelWidth = Camera.main.pixelWidth;
            cameraPixelHeight = Camera.main.pixelHeight;
            DontDestroyOnLoad(this);
            Setup();
        }

        void Update()
        {
            ProcessEvents();
        }

        public virtual void Setup()
        {
            // for the children classes
        }

        public TuioInputController InputController()
        {
            return tuioInput;
        }

        // Ensure that the instance is destroyed when the game is stopped in the editor.
        void OnApplicationQuit()
        {
            if (tuioInput != null)
            {
                tuioInput.m_collectEvents = false;
                tuioInput.disconnect();
            }
        }

        private void UpdateEvent(TuioTouchEvent anEvent, TuioCursor cursor)
        {
            anEvent.m_lastScreenPosition = anEvent.m_screenPosition;
            anEvent.m_tuioPosition = new Vector2(cursor.getX(), (1.0f - cursor.getY()));
            anEvent.m_screenPosition = new Vector3(cursor.getX() * cameraPixelWidth, (1.0f - cursor.getY()) * cameraPixelHeight, 0.3f);
            anEvent.m_lastTouchTime = anEvent.m_touchTime;
            anEvent.m_touchTime = Time.time;
            anEvent.m_didChange = true;
        }

        // Cursor down is for new touch events. we take the TUIO cursor object and convert it
        // into a touch event, and add it to our active list of events
        public virtual void CursorDown(TuioCursor cursor)
        {
            // first, make a new BBTouchEvent, tag it with the unique touch id
            TuioTouchEvent newEvent = new TuioTouchEvent(cursor.getSessionID());
            // set the initial information		
            newEvent.m_screenPosition = new Vector3(cursor.getX() * cameraPixelWidth, (1.0f - cursor.getY()) * cameraPixelHeight, 0.3f);
            newEvent.m_eventState = TouchEventState.Began;
            newEvent.m_didChange = true;
            // set all the rest of the info
            UpdateEvent(newEvent, cursor);

            // add it to our active event dictionary so we can retireve it based on it's unique ID
            // some times badness happens and we get an error adding one here for some reason
            // it should not ever be the case that the ID is already there.
            // if it is, then we need to behave
            if (m_activeEvents.ContainsKey(cursor.getSessionID()))
            {
                // then something is not right.. remove the old one and add a new one
                m_activeEvents.Remove(cursor.getSessionID());
            }
            m_activeEvents.Add(cursor.getSessionID(), newEvent);
            // queue it up for processing
            lock (m_eventQueueLock) m_eventQueue.Add(newEvent);
        }

        public virtual void CursorMove(TuioCursor cursor)
        {
            // find the matching event object, set th state to 'moved'
            // and update it with the new position info
            if (!m_activeEvents.ContainsKey(cursor.getSessionID())) return;
            TuioTouchEvent anEvent = m_activeEvents[cursor.getSessionID()];
            UpdateEvent(anEvent, cursor);
            anEvent.m_eventState = TouchEventState.Moved;
            lock (m_eventQueueLock) m_eventQueue.Add(anEvent);
        }

        public virtual void CursorUp(TuioCursor cursor)
        {
            // find the matching event object, set the state to 'ended'
            // and remove it from our actives
            if (!m_activeEvents.ContainsKey(cursor.getSessionID())) return;
            TuioTouchEvent anEvent = m_activeEvents[cursor.getSessionID()];
            anEvent.m_eventState = TouchEventState.Ended;
            lock (m_eventQueueLock) m_eventQueue.Add(anEvent);
            m_activeEvents.Remove(cursor.getSessionID());
        }

        public void ProcessEvents()
        {
            ArrayList events = tuioInput.getAndClearCursorEvents();
            // go through the events and dispatch
            foreach (CursorEvent cursorEvent in events)
            {
                if (cursorEvent.m_state == CursorState.Add)
                {
                    CursorDown(cursorEvent.m_cursor);
                    continue;
                }
                if (cursorEvent.m_state == CursorState.Update)
                {
                    CursorMove(cursorEvent.m_cursor);
                    continue;
                }
                if (cursorEvent.m_state == CursorState.Remove)
                {
                    CursorUp(cursorEvent.m_cursor);
                    continue;
                }
            }
            FinishFrame();
        }

        public virtual void FinishFrame()
        {
            // this is called when the TUIO fseq message comes through, and it is
            // the end of this cycle.
            // normally you would process the event Q here
            lock (m_eventQueueLock) m_eventQueue.Clear();
            foreach (TuioTouchEvent touch in m_activeEvents.Values)
            {
                // any unchanging events need to have their screen position updated
                // any changing events need to be set to unchanged
                // for the next round
                if (touch.m_didChange)
                {
                    touch.m_didChange = false;
                }
                else
                {
                    touch.m_lastScreenPosition = touch.m_screenPosition;
                }
            }
        }
    }
}
