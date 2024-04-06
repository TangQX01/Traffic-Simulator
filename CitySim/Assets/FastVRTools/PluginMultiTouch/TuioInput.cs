using UnityEngine;
using System.Collections;

namespace SIP.FastVRTools.MultiTouch
{
	public class TuioTouch
	{
		public int fingerId;
		public Vector2 position;
		public Vector2 deltaPosition;
		public float deltaTime;
		public int tapCount; // not supported at the moment
		public TuioTouchPhase phase;
	}

	public enum TuioTouchPhase 
	{
		Began,
		Moved,
		Stationary,
		Ended,
		Canceled
	};

    public static class TuioInput
    {
		public static TuioInputManager m_tuioInputManager = (new GameObject("TuioManager")).AddComponent<TuioInputManager>();
		//public static TuioInputThread m_tuioThread = new TuioInputThread();
		public static bool multiTouchEnabled = true;
		
		public static TuioTouch GetTouch(int index)
		{
			return TuioInputManager.instance.activeIphoneTouches[index] as TuioTouch;	
			//return TuioInputThread.activeIphoneTouches[index] as TuioTouch;
		}
		
		public static TuioTouch[] touches
		{
			get {
				return (TuioTouch[])TuioInputManager.instance.activeIphoneTouches.ToArray(typeof(TuioTouch));
				//return (TuioTouch[])TuioInputThread.activeIphoneTouches.ToArray(typeof(TuioTouch));
			}
		}
		
		public static int touchCount
		{
			get {
				return TuioInputManager.instance.activeIphoneTouches.Count;
				//return TuioInputThread.activeIphoneTouches.Count;
			}
		}

    }
}
