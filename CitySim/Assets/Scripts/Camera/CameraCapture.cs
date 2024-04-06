using System;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class CameraCapture : MonoBehaviour
{
    public GameObject CarGenerator;
    public List<Camera> m_CamList = new List<Camera>();
    public float m_TimePass = 0.0f;
    public float m_ReCaptureTime = 10.0f;
    /// <summary>
    /// 对相机截图
    /// </summary>
    /// <param name="camera">Camera.要被截屏的相机</param>
    /// <param name="rect">Rect.截屏的区域</param>
    /// <returns>The screenshot2.</returns>
    Texture2D CaptureCamera(Camera camera, Rect rect, string save_name)
    {
        var origin_targetTexture = camera.targetTexture;
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
        camera.targetTexture = origin_targetTexture;
        RenderTexture.active = null;
        Destroy(rt);

        //byte[] bytes = screenShot.EncodeToPNG();//最后将这些纹理数据，成一个png图片文件
        //string filename = Application.dataPath + "/CameraCapture/" + save_name;
        string nowtime = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
        string image_name = string.Format("{0}+{1}.jpg", camera.name, nowtime);
        string filename = Application.dataPath + "/CameraCapture/" + save_name + "/" + image_name;
        saveTexture(filename, t2D);
        //System.IO.File.WriteAllBytes(filename, bytes);
        //Debug.Log(string.Format("截屏了一张照片: {0}", filename));

        return t2D;
    }

    /// <summary> 保存贴图 </summary>
     /// <param name="path">保存路径</param>
     /// <param name="texture">Texture2D</param>
    public static void saveTexture(string path, Texture2D texture)
    {
        File.WriteAllBytes(path, texture.EncodeToPNG());
//#if UNITY_EDITOR
//        Debug.Log("已保存截图到:" + path);
//#endif
    }
    public void Start()
    {
        for (int i = 0; i < m_CamList.Count; i++)
        {
            print(m_CamList[i].name);
        }
    }

    public void Update()
    {
        m_TimePass += Time.deltaTime;
        if (m_TimePass <= m_ReCaptureTime)
            return;
        m_TimePass = 0.0f;
        for (int i = 0; i < m_CamList.Count; i++)
        {
            //print(CarGenerator.transform.GetChild(i).name);
            //print((CarGenerator.transform.GetChild(i).transform.Find("CarRecorder")).GetComponent<Camera>().rect);
            Camera tmp_camera = m_CamList[i];
            string save_name = tmp_camera.name;
            //Rect rect = new Rect(tmp_camera.rect.x*Screen.width, tmp_camera.rect.y*Screen.height, tmp_camera.rect.width*Screen.width, tmp_camera.rect.height*Screen.height);
            //Rect rect = new Rect(0, 0, Screen.width * tmp_camera.rect.width, Screen.height * tmp_camera.rect.height);
            //Rect rect = new Rect(0, 0, Screen.width, Screen.height);
            //Rect rect = new Rect(0, 0, 1080, 1080);
            Rect rect = new Rect(0, 0, 2560, 2560);
            CaptureCamera(tmp_camera, rect, save_name);
        }
        //for (int i = 0; i < CarGenerator.transform.childCount; i++)
        //{
        //    //print(CarGenerator.transform.GetChild(i).name);
        //    //print((CarGenerator.transform.GetChild(i).transform.Find("CarRecorder")).GetComponent<Camera>().rect);
        //    Camera tmp_camera = (CarGenerator.transform.GetChild(i).transform.Find("CarRecorder")).GetComponent<Camera>();
        //    string save_name = CarGenerator.transform.GetChild(i).name;
        //    //Rect rect = new Rect(tmp_camera.rect.x*Screen.width, tmp_camera.rect.y*Screen.height, tmp_camera.rect.width*Screen.width, tmp_camera.rect.height*Screen.height);
        //    //Rect rect = new Rect(0, 0, Screen.width * tmp_camera.rect.width, Screen.height * tmp_camera.rect.height);
        //    //Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        //    Rect rect = new Rect(0, 0, 1080, 1080);
        //    CaptureCamera(tmp_camera, rect, save_name);
        //}
    }
}
