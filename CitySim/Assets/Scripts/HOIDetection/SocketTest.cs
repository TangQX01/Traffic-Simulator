using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;
using UnityEngine;
using System.Text;
using System.IO;

public class SocketTest : MonoBehaviour
{
    public Camera m_camera;
    private int i;
    private bool iscon = false;
    private Socket m_socket;
    private byte[] m_sendBuff;
    private byte[] m_recvBuff;
    private AsyncCallback m_recvCb;
    // Start is called before the first frame update
    void Start()
    {
        m_recvBuff = new byte[1024000];
        m_sendBuff = new byte[1024000];
        m_recvCb = new AsyncCallback(RecvCallBack);
        //*******************************************************************
        //������
        Socket lfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //����IP��ַ�Ͷ˿ںŶ���
        IPAddress ip = IPAddress.Any; //IPAddress.Parse("127.0.0.1");
        //�˿ں�
        IPEndPoint point = new IPEndPoint(ip, 6666);
        //�ø��������socket��IP��ַ���˿ں�
        lfd.Bind(point);
        Debug.Log("��������������");
        lfd.Listen(5);
        Thread th = new Thread(Listen);
        th.IsBackground = true;
        th.Start(lfd);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            string nowtime = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            Debug.Log("����������˲��");
            screenShoot(m_camera);
            //socket������Ϣ
            //m_sendBuff = Encoding.UTF8.GetBytes(nowtime);
            //NetworkStream netstream = new NetworkStream(m_socket);
            //netstream.Write(m_sendBuff, 0, m_sendBuff.Length);
        }
    }

    private void screenShoot(Camera camera)
    {
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        // ����һ��RenderTexture����
        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
        // ��ʱ������������targetTextureΪrt, ���ֶ���Ⱦ������
        camera.targetTexture = rt;
        camera.Render();
        // �������rt, �������ж�ȡ���ء�
        RenderTexture.active = rt;
        Texture2D t2D = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        t2D.ReadPixels(rect, 0, 0);// ע�����ʱ�����Ǵ�RenderTexture.active�ж�ȡ����
        t2D.Apply();
        // ������ز�������ʹ��camera��������Ļ����ʾ
        camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        // �����Щ�������ݣ���һ��pngͼƬ�ļ�
        byte[] bytes = t2D.EncodeToJPG();
        Debug.Log(bytes.Length);
        //byte[] bytes = t2D.EncodeToPNG();
        string nowtime = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
        //string save_path = "Assets/CameraCapture/" + nowtime + ".jpg";
        //File.WriteAllBytes(save_path, bytes);
        //Debug.Log("��ͼ���浽: "+save_path);
        //socket������Ϣ
        //m_sendBuff = Encoding.UTF8.GetBytes(save_path);
        m_sendBuff = bytes;
        NetworkStream netstream = new NetworkStream(m_socket);
        netstream.Write(m_sendBuff, 0, m_sendBuff.Length);
    }

    void Listen(object o)
    {
        Socket lfd = o as Socket;
        while (true)
        {
            Socket rfd = lfd.Accept();
            Debug.Log(rfd.RemoteEndPoint.ToString() + "��������");
            m_socket = rfd;
            iscon = true;
        }

    }

    private void FixedUpdate()
    {
        if (iscon == true)
        {
            m_socket.BeginReceive(m_recvBuff, 0, m_recvBuff.Length, SocketFlags.None, m_recvCb, this);
        }
    }

    void RecvCallBack(IAsyncResult ar)
    {
        var len = m_socket.EndReceive(ar);
        byte[] msg = new byte[len];
        Array.Copy(m_recvBuff, msg, len);
        string msgStr = System.Text.Encoding.UTF8.GetString(msg);
        Debug.Log("��û���յ���" + msgStr);
        for (int i = 0; i < m_recvBuff.Length; ++i)
        {
            m_recvBuff[i] = 0;
        }
    }
}
