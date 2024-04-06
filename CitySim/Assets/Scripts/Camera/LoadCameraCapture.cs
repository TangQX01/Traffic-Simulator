using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class LoadCameraCapture : MonoBehaviour
{
    public Image image;//用来显示图片
    private Sprite sprite;//粗放sprite类型的图片
    private string filepath = @"E:\Project\city-simulator\code\unity\CitySim\Assets\CameraCapture\";
    public string filename = "Car 0000.jpg";


    /// <summary>
    /// 从外部指定文件中加载图片
    /// </summary>
    /// <returns></returns>
    private Texture2D LoadTextureByIO()
    {
        FileStream fs = new FileStream(filepath + filename, FileMode.Open, FileAccess.Read);
        //FileStream fs = new FileStream(@"E:\Project\Unity\citysimulator\Assets\My\CameraCapture\cameraCapture.jpg", FileMode.Open, FileAccess.Read);
        fs.Seek(0, SeekOrigin.Begin);//游标的操作，可有可无
        byte[] bytes = new byte[fs.Length];//生命字节，用来存储读取到的图片字节
        try
        {
            fs.Read(bytes, 0, bytes.Length);//开始读取，这里最好用trycatch语句，防止读取失败报错

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        fs.Close();//切记关闭

        int width = 2048;//图片的宽（这里两个参数可以提到方法参数中）
        int height = 2048;//图片的高（这里说个题外话，pico相关的开发，这里不能大于4k×4k不然会显示异常，当时开发pico的时候应为这个问题找了大半天原因，因为美术给的图是6000*3600，导致出现切几张图后就黑屏了。。。
        Texture2D texture = new Texture2D(width, height);
        if (texture.LoadImage(bytes))
        {
            //print("图片加载完毕 ");
            return texture;//将生成的texture2d返回，到这里就得到了外部的图片，可以使用了

        }
        else
        {
            print("图片尚未加载");
            return null;
        }
    }


    /// <summary>
    /// 将Texture2d转换为Sprite
    /// </summary>
    /// <param name="tex">参数是texture2d纹理</param>
    /// <returns></returns>
    private Sprite TextureToSprite(Texture2D tex)
    {
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sprite = TextureToSprite(LoadTextureByIO());
        image.sprite = sprite;
    }
}
