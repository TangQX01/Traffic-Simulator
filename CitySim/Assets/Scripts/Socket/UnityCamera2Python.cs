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
            //������
            Socket lfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //����IP��ַ�Ͷ˿ںŶ���
            IPAddress ip = IPAddress.Any; //IPAddress.Parse("127.0.0.1");
                                          //�˿ں�
            IPEndPoint point = new IPEndPoint(ip, port);
            //�ø��������socket��IP��ַ���˿ں�
            lfd.Bind(point);
            Debug.Log("��������������");
            lfd.Listen(5);
            Thread th = new Thread(Listen);
            th.IsBackground = true;
            th.Start(lfd);
        }

        public void CameraCapture( Camera camera)
        {
            var origin_targetTexture = camera.targetTexture;
            Rect rect = new Rect(0, 0, Screen.width, Screen.height);
            // ����һ��RenderTexture����
            //RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
            RenderTexture rt = new RenderTexture(1920, 1080, 0);
            // ��ʱ������������targetTextureΪrt, ���ֶ���Ⱦ������
            camera.targetTexture = rt;
            camera.Render();
            // �������rt, �������ж�ȡ���ء�
            RenderTexture.active = rt;
            Texture2D t2D = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
            t2D.ReadPixels(rect, 0, 0);// ע�����ʱ�����Ǵ�RenderTexture.active�ж�ȡ����
            t2D.Apply();
            // ������ز�������ʹ��camera��������Ļ����ʾ
            camera.targetTexture = origin_targetTexture;
            RenderTexture.active = null;
            Destroy(rt);
            // �����Щ�������ݣ���һ��pngͼƬ�ļ�

            byte[] bytes = t2D.EncodeToJPG();
            //byte[] bytes = t2D.EncodeToPNG();


            //socket������Ϣ
            m_sendBuff = bytes;
            NetworkStream netstream = new NetworkStream(m_socket);
            netstream.Write(m_sendBuff, 0, m_sendBuff.Length);
            print(string.Format("�Ѵ��䳤��: {0}", bytes.Length));
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
            Debug.Log("�Ƿ��յ���" + msgStr);
            isRecv = true;
            for (int i = 0; i < m_recvBuff.Length; ++i)
            {
                m_recvBuff[i] = 0;
            }
        }


        /// <summary>
        /// Tax: Э�̴���
        /// </summary>
        public IEnumerator CameraTexture2Python(Camera camera)
        {
            //CameraCapture(camera);
            //print(camera.name + "��ͼ���ϴ���python");
            //yield return null;
            while (true)
            {
                yield return new WaitUntil(() => isRecv == true);
                //yield return new WaitForSecondsRealtime(1);
                CameraCapture(camera);
                print(camera.name + "��ͼ���ϴ���python");
                isRecv = false;
            }
        }
    }
}
