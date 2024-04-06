//using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SIP.FastVRTools.MultiTouch.TUIO;

namespace SIP.FastVRTools.MultiTouch
{
    public enum CursorState
    {
        Add,
        Update,
        Remove
    };

    public class CursorEvent
    {
        public TuioCursor m_cursor;
        public CursorState m_state;

        public CursorEvent(TuioCursor c, CursorState s)
        {
            m_cursor = c;
            m_state = s;
        }
    }

    public class TuioInputController : TuioListener
    {
        private TuioClient m_client;

        public ArrayList m_activeCursorEvents = new ArrayList();

        private object m_objectSync = new object();
        public bool m_collectEvents = false;

        public TuioInputController()
        {
            m_client = new TuioClient(3333);
            m_client.addTuioListener(this);
            m_client.connect();
        }

        public ArrayList getAndClearCursorEvents()
        {
            ArrayList bufferList;
            lock (m_objectSync)
            {
                bufferList = new ArrayList(m_activeCursorEvents);
                m_activeCursorEvents.Clear();
            }
            return bufferList;
        }

        public void disconnect()
        {
            m_client.disconnect();
            m_client.removeTuioListener(this);
        }

        public bool isConnected()
        {
            return m_client.isConnected();
        }

        public int currentFrame()
        {
            return m_client.currentFrameNumber();
        }

        public string getStatusString()
        {
            return m_client.getStatusString();
        }

        // required implementations	
        public void addTuioObject(TuioObject o)
        {
            // if (eventDelegate) eventDelegate.objectAdd(o);	
        }

        public void updateTuioObject(TuioObject o)
        {
            // if (eventDelegate) eventDelegate.objectUpdate(o);	
        }

        public void removeTuioObject(TuioObject o)
        {
            // if (eventDelegate) eventDelegate.objectRemove(o);
        }
        // 
        // for now we are only interested in cursor objects, ie touch events
        public void addTuioCursor(TuioCursor c)
        {
            lock (m_objectSync)
            {
                if (m_collectEvents) m_activeCursorEvents.Add(new CursorEvent(c, CursorState.Add));
            }
        }

        public void updateTuioCursor(TuioCursor c)
        {
            lock (m_objectSync)
            {
                if (m_collectEvents) m_activeCursorEvents.Add(new CursorEvent(c, CursorState.Update));
            }
        }

        public void removeTuioCursor(TuioCursor c)
        {
            lock (m_objectSync)
            {
                if (m_collectEvents) m_activeCursorEvents.Add(new CursorEvent(c, CursorState.Remove));
            }
        }

        // this is the end of a single frame
        public void refresh(TuioTime ftime)
        {
            // we dont need to do anything here really
        }
    }
}
