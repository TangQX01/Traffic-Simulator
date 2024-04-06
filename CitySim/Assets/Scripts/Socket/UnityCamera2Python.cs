using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;
using UnityEngine;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using UnityStandardAssets.Utility;

namespace CitySim{
    public class UnityCamera2Python : MonoBehaviour
    {
        public Camera m_Camera;
        public GameObject m_CameraManager;
        public List<Camera> m_CamList = new List<Camera>();
        public int i;
        public bool iscon = false;
        private bool isRecv = true;
        public Socket m_socket;
        public byte[] m_sendBuff;
        public byte[] m_recvBuff;
        private bool isStartCoroutine = false;
        public AsyncCallback m_recvCb;
        // Start is called before the first frame update
        void Start()
        {
            SocketStart(6667);
        }

        // Update is called once per frame
        void Update()
        {
            if (isStartCoroutine == false && iscon == true)
            {
                StartCoroutine(CameraTexture2Python(m_Camera));
                isStartCoroutine = true;
            }
        }
        public void SocketStart(int port)
        {
            m_recvBuff = new byte[102400];
            m_sendBuff = new byte[102400];
            m_recvCb = new AsyncCallback(RecvCallBack);
            //*******************************************************************
            //服务器
            Socket lfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //创建IP地址和端口号对象
            IPAddress ip = IPAddress.Any; //IPAddress.Parse("127.0.0.1");
                                          //端口号
            IPEndPoint point = new IPEndPoint(ip, port);
            //让负责监听的socket绑定IP地址跟端口号
            lfd.Bind(point);
            Debug.Log("服务器监听启动");
            lfd.Listen(5);
            Thread th = new Thread(Listen);
            th.IsBackground = true;
            th.Start(lfd);
        }

        public void CameraCapture( Camera camera)
        {
            var origin_targetTexture = camera.targetTexture;
            Rect rect = new Rect(0, 0, Screen.width, Screen.height);
            // 创建一个RenderTexture对象
            //RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
            RenderTexture rt = new RenderTexture(1920, 1080, 0);
            // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机
            camera.targetTexture = rt;
            camera.Render();
            // 激活这个rt, 并从中中读取像素。
            RenderTexture.active = rt;
            Texture2D t2D = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
            t2D.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素
            t2D.Apply();
            // 重置相关参数，以使用camera继续在屏幕上显示
            camera.targetTexture = origin_targetTexture;
            RenderTexture.active = null;
            Destroy(rt);
            // 最后将这些纹理数据，成一个png图片文件

            byte[] bytes = t2D.EncodeToJPG();
            //byte[] bytes = t2D.EncodeToPNG();


            //socket发送信息
            m_sendBuff = bytes;
            NetworkStream netstream = new NetworkStream(m_socket);
            netstream.Write(m_sendBuff, 0, m_sendBuff.Length);
            print(string.Format("已传输长度: {0}", bytes.Length));
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
            Debug.Log("是否收到：" + msgStr);
            isRecv = true;
            for (int i = 0; i < m_recvBuff.Length; ++i)
            {
                m_recvBuff[i] = 0;
            }
        }


        /// <summary>
        /// Tax: 协程代码
        /// </summary>
        public IEnumerator CameraTexture2Python(Camera camera)
        {
            //CameraCapture(camera);
            //print(camera.name + "截图已上传给python");
            //yield return null;
            while (true)
            {
                yield return new WaitUntil(() => isRecv == true);
                //yield return new WaitForSecondsRealtime(1);
                CameraCapture(camera);
                print(camera.name + "截图已上传给python");
                isRecv = false;
            }
        }
    }
}
