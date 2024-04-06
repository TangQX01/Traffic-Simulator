using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIP.FastVRTools.Cameras
{
    public class CameraTrigger : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            print(transform.GetComponent<FollowCamera>().m_followObject.name);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
