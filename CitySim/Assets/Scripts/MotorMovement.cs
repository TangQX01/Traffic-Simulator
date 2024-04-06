using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorMovement : MonoBehaviour
{
    public float m_MoveSpeed = 6;
    public float m_RotSpeed = 1;
    public Transform m_Target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float step = m_MoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, m_Target.position, step);
    }

    void MotorControl()
    {
        if (Input.GetKey("w"))
        {
            //transform.position += new Vector3(0, 0, m_MoveSpeed);
            transform.Translate(new Vector3(0, 0, m_MoveSpeed));
        }

        if(Input.GetKey("s"))
        {
            //transform.position -= new Vector3(0, 0, m_MoveSpeed);
            transform.Translate(new Vector3(0, 0, -m_MoveSpeed));
        }

        if(Input.GetKey("a"))
        {
            transform.RotateAround(transform.position, transform.up, m_RotSpeed);
        }

        if(Input.GetKey("d"))
        {
            transform.RotateAround(transform.position, -transform.up, m_RotSpeed);
        }
    }
}
