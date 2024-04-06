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
using FrameWorkSongDemo;


namespace CitySim
{
    public struct SendMessege
    {
        //public byte[] image_bytes;
        public string camera_name;          //��ͼ���������
        public string position_x;           //��ͼʱ������x
        public string position_z;           //��ͼʱ������y
        public string capture_time;         //��ͼʱ���ʱ��
        public string image_name;           //��ͼ������
        public string save_path;            //��ͼ�����·��
    };
    public struct ReceiveMessage
    {
        /// <summary>
        /// qingxian: �������������python�е�һ��
        /// </summary>
        public string type_violation;       //Υ������
        public string image_save_path;      //ʶ����ͼƬ�洢λ��
        public string position_x;           //��ͼʱ������x
        public string position_z;           //��ͼʱ������y
    }
    public class SocketTest : MonoBehaviour
    {
        //public GameObject m_CameraManager;
        //public List<Camera> m_CamList = new List<Camera>();
        public int i;
        public bool iscon = false;
        private bool isRecv = true;
        private bool isStartCoroutine = false;
        public Socket m_socket;
        public byte[] m_sendBuff;
        public byte[] m_recvBuff;
        public AsyncCallback m_recvCb;
        public Camera m_Camera;
        public GameObject m_HeatmapCubes;
        [HideInInspector]
        public List<HeatMapData> m_Cubes = new List<HeatMapData>();
        private float m_Radius = 50;
        public Queue<ReceiveMessage> m_recvmsg = new Queue<ReceiveMessage>();
        // Start is called before the first frame update
        void Start()
        {
            SocketStart(6666);
            for (int i = 0; i < m_HeatmapCubes.transform.childCount; i++)
            {
                HeatMapData cube = m_HeatmapCubes.transform.GetChild(i).GetComponent<HeatMapData>();
                if (cube)
                {
                    m_Cubes.Add(cube);
                }
            }
            //ReceiveMessage recv_message = new ReceiveMessage();
            //recv_message.position_x = "-341.75";
            //recv_message.position_z = "-268.1";
            //DealReceiveMessage(recv_message);
            //FindIndexInCubes(-341.75f, -268.1f);
            //foreach (Transform item in m_HeatmapCubes.transform)
            //{
            //    HeatMapData cube = item.GetComponent<HeatMapData>();
            //    m_Cubes.Add(cube);
            //}
            /// <summary>
            /// Tqx: Ϊÿ��Camera����һ����ͼ��Э��
            /// </summary>
            //StartCoroutine(WaitHOIDetection(m_Camera));
            //foreach (Transform item in m_CameraManager.transform)
            //{
            //    var cam = item.GetComponent<Camera>();
            //    print(cam.transform.name);
            //    if (cam.gameObject.activeSelf && cam.rect.width == 1)
            //    {
            //        m_CamList.Add(cam);
            //        StartCoroutine(WaitHOIDetection(cam));
            //    }
            //}
        }


        // Update is called once per frame
        void Update()
        {
            while (m_recvmsg.Count != 0)
            {
                ReceiveMessage recv_msg = m_recvmsg.Dequeue();
                DealReceiveMessage(recv_msg);
            }
            if (isStartCoroutine == false && iscon == true)
            {
                /// <summary>
                /// Tqx: Ϊÿ��Camera����һ����ͼ��Э��
                /// </summary>
                StartCoroutine(WaitHOIDetection(m_Camera));
                //foreach (Transform item in m_CameraManager.transform)
                //{
                //    var cam = item.GetComponent<Camera>();
                //    print(cam.transform.name);
                //    if (cam.gameObject.activeSelf && cam.rect.width == 1)
                //    {
                //        m_CamList.Add(cam);
                //        StartCoroutine(WaitHOIDetection(cam));
                //    }
                //}
                isStartCoroutine = true;
            }
        }
        float Pow2(float x)
        {
            return x * x;
        }

        public int FindIndexInCubes(float x, float z)
        {
            for (int i = 0; i < m_Cubes.Count; i++)
            {
                HeatMapData cube = m_Cubes[i];
                float posX = m_Cubes[i].transform.position.x;
                float posZ = m_Cubes[i].transform.position.z;
                if (Pow2(posX - x) + Pow2(posZ - z) < Pow2(m_Radius))
                {
                    return i;
                }
            }
            return -1;
        }

        public void DealReceiveMessage(ReceiveMessage recv_message)
        {
            if (recv_message.type_violation == "δ����Υ����Ϊ")
            {
                return;
            }
            int index = FindIndexInCubes(float.Parse(recv_message.position_x), float.Parse(recv_message.position_z));
            Debug.Log("index: " + index.ToString());
            if (index == -1)
            {
                Debug.Log("��ǰ����û���ҵ���Ӧ���ȵ�");
            }
            else
            {
                Debug.Log("�±��ǣ�" + index.ToString());
                m_Cubes[index].Amount += 150;
            }
        }
        //public void DealReceiveMessage(ReceiveMessage recv_message)
        //{
        //    Debug.Log("DealReceiveMessage");
        //    //if (recv_message.type_violation == "δ����Υ����Ϊ")
        //    //{
        //    //    return ;
        //    //}
        //    int index = FindIndexInCubes(float.Parse(recv_message.position_x), float.Parse(recv_message.position_z));
        //    Debug.Log("index: " + index.ToString());
        //    if (index == -1)
        //    {
        //        Debug.Log("��ǰ����û���ҵ���Ӧ���ȵ�");
        //    }
        //    else 
        //    {
        //        Debug.Log("�±��ǣ�" + index.ToString());
        //        m_Cubes[index].Amount += 100;
        //    }
        //}

        public void SocketStart(int port)
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
            IPEndPoint point = new IPEndPoint(ip, port);
            //�ø��������socket��IP��ַ���˿ں�
            lfd.Bind(point);
            Debug.Log("��������������");
            lfd.Listen(5);
            Thread th = new Thread(Listen);
            th.IsBackground = true;
            th.Start(lfd);
        }

        public void screenShoot(Camera camera)
        {
            var origin_targetTexture = camera.targetTexture;
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
            camera.targetTexture = origin_targetTexture;
            RenderTexture.active = null;
            Destroy(rt);
            // �����Щ�������ݣ���һ��pngͼƬ�ļ�
            byte[] bytes = t2D.EncodeToJPG();
            Debug.Log(bytes.Length);
            //byte[] bytes = t2D.EncodeToPNG();
            string nowtime = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            //string image_name = string.Format("{0}+{1}+{2}+{3}.jpg", camera.name, camera.transform.position.x, camera.transform.position.z, nowtime);
            string image_name = string.Format("{0}_{1}.jpg", camera.name, nowtime);


            string save_path = Application.dataPath + '\\' + camera.name;
            if (!Directory.Exists(save_path))
            {
                Directory.CreateDirectory(save_path);
            }
            save_path = save_path + "\\" + image_name;
            //string save_path = "Assets/CameraCapture/" + image_name;
            //string save_path = "E:\\Project\\city-simulator\\code\\unity\\CitySim\\Assets\\CameraCapture\\" + camera.name + "\\" + image_name;
            SendMessege CM = new SendMessege();
            //CM.image_bytes = bytes;
            CM.camera_name = camera.name;
            CM.position_x = camera.transform.position.x.ToString();
            CM.position_z = camera.transform.position.z.ToString();
            CM.capture_time = nowtime;
            CM.save_path = save_path;
            CM.image_name = image_name;
            File.WriteAllBytes(save_path, bytes);
            Debug.Log("��ͼ���浽: " + save_path);
            //socket������Ϣ
            //m_sendBuff = Encoding.UTF8.GetBytes(save_path + "end_message");
            //m_sendBuff = bytes;

            /// <summary>
            /// qingxian: �ṹ���string���໥ת��
            /// string strJson = JsonConvert.SerializeObject(testInfo);
            /// TestInfo testInfo = JsonConvert.DeserializeObject<TestInfo>(strJson);
            /// </summary>
            string sendMessage = JsonConvert.SerializeObject(CM);
            m_sendBuff = Encoding.UTF8.GetBytes(sendMessage + "end_message");
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
            string[] recv_msg_all = msgStr.Split("end_message");
            //Debug.Log("�յ�����" + msgStr);
            foreach (string _recv_msg in recv_msg_all)
            {
                //Debug.Log(_recv_msg.Length);
                if (_recv_msg.Length <= 0)
                {
                    continue;
                }
                Debug.Log(_recv_msg);
                ReceiveMessage recv_msg = JsonConvert.DeserializeObject<ReceiveMessage>(_recv_msg);
                m_recvmsg.Enqueue(recv_msg);
            }
            isRecv = true;
            for (int i = 0; i < m_recvBuff.Length; ++i)
            {
                m_recvBuff[i] = 0;
            }
        }


        /// <summary>
        /// Tax: Э�̴���
        /// </summary>

        public IEnumerator WaitHOIDetection(Camera camera)
        {
            while (true)
            {
                yield return new WaitUntil(() => isRecv == true);
                //print(camera.name + "��ʼ��ͼ");
                screenShoot(camera);
                yield return new WaitForSeconds(10);
            }
        }
    }
}