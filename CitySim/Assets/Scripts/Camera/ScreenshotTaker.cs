using UnityEngine;  
  
public class ScreenshotTaker : MonoBehaviour  
{  
    public string screenshotName = "BicycleAndPeople"; // 截图文件名  
    private int m_FrameCnt = 0;
    private int m_ShotCnt = 0;
  
    void Update()  
    {  
        m_FrameCnt += 1;
        // if (m_FrameCnt%100 == 0){  
        //     TakeScreenshot(screenshotName + m_FrameCnt);  
        // }  
        
        // 检查是否按下了截图快捷键  
        if (Input.GetKeyDown(KeyCode.P))  {
            TakeScreenshot("OSM_"+m_ShotCnt);
            m_ShotCnt += 1;
        }
    }  
  
    void TakeScreenshot(string name)  
    {  
        string filename = name + ".png";  
          
        // 截图并保存  
        ScreenCapture.CaptureScreenshot(filename);  
        Debug.Log("Screenshot taken and saved as " + filename);  
    }  
}