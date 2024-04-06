using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SIP.FastVRTools;

public class ClickTag : MonoBehaviour
{
    public FVRCameraManager cam_manager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Camera cam = cam_manager.m_cameras[cam_manager.m_currentCamNum];
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.gameObject == this.gameObject)
                {
                    cam_manager.SwitchCameraByName(transform.name);
                }
            }
        }
    }
}
