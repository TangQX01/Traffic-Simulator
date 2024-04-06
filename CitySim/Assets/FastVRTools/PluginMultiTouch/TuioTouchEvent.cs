using UnityEngine;
using System.Collections;

namespace SIP.FastVRTools.MultiTouch
{
    public enum TouchEventState
    {
        Began,
        Moved,
        Stationary,
        Ended
    };

    public class TuioTouchEvent
    {
        /// <summary>
        /// Touch event status
        /// </summary>
        public TouchEventState m_eventState;
        public bool m_didChange = false;

        /// <summary>
        /// This is touch id
        /// </summary>
		public long m_eventID;

        /// <summary>
        /// The symbolID for this touch, it is unique for each symbol and set by convention to -1 for cursors
        /// </summary>
        public long m_symbolID;

        /// <summary>
        /// The 2d position of this touch normalized to 0..1,0..1
        /// </summary>
        public Vector2 m_tuioPosition;

        /// <summary>
        /// The last 2d position of this touch on the screen
        /// </summary>
        public Vector3 m_lastScreenPosition;

        /// <summary>
        /// The 2d position of this touch on the screen
        /// </summary>
        public Vector3 m_screenPosition;

//         public Vector3 rayCastHitPosition; // the 3d point where this touch event ray cast into teh scene and collided with something
//         public Vector3 lastRayCastHitPosition; // the previous hit location

        /// <summary>
        /// Current touch time.
        /// </summary>
        public float m_touchTime;

        /// <summary>
        /// Last touch time.
        /// </summary>
        public float m_lastTouchTime;

        public TuioTouchEvent(long id)
        {
			this.m_eventID = id;
            this.m_symbolID = -1;
        }

        public TuioTouchEvent(long id, long symbol)
        {
			this.m_eventID = id;
            this.m_symbolID = symbol;
        }
    }
}
