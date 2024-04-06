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
        //服务器
        Socket lfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //创建IP地址和端口号对象
        IPAddress ip = IPAddress.Any; //IPAddress.Parse("127.0.0.1");
        //端口号
        IPEndPoint point = new IPEndPoint(ip, 6666);
        //让负责监听的socket绑定IP地址跟端口号
        lfd.Bind(point);
        Debug.Log("服务器监听启动");
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
            Debug.Log("鼠标左键按下瞬间");
            screenShoot(m_camera);
            //socket发送信息
            //m_sendBuff = Encoding.UTF8.GetBytes(nowtime);
            //NetworkStream netstream = new NetworkStream(m_socket);
            //netstream.Write(m_sendBuff, 0, m_sendBuff.Length);
        }
    }

    private void screenShoot(Camera camera)
    {
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        // 创建一个RenderTexture对象
        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
        // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机
        camera.targetTexture = rt;
        camera.Render();
        // 激活这个rt, 并从中中读取像素。
        RenderTexture.active = rt;
        Texture2D t2D = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        t2D.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素
        t2D.Apply();
        // 重置相关参数，以使用camera继续在屏幕上显示
        camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        // 最后将这些纹理数据，成一个png图片文件
        byte[] bytes = t2D.EncodeToJPG();
        Debug.Log(bytes.Length);
        //byte[] bytes = t2D.EncodeToPNG();
        string nowtime = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
        //string save_path = "Assets/CameraCapture/" + nowtime + ".jpg";
        //File.WriteAllBytes(save_path, bytes);
        //Debug.Log("截图保存到: "+save_path);
        //socket发送信息
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
            Debug.Log(rfd.RemoteEndPoint.ToString() + "已连接了");
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
        Debug.Log("有没有收到：" + msgStr);
        for (int i = 0; i < m_recvBuff.Length; ++i)
        {
            m_recvBuff[i] = 0;
        }
    }
}
