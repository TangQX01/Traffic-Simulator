using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Diagnostics;
using System;

public class test_RunPython : MonoBehaviour
{
    public string PythonName;
    string sArguments = @"HelloWorld.py";//这里是python的文件名字
    Process yoloDetect;

    public static Process RunPythonScript(string sArgName, string args = "")
    {
        Process p = new Process();
        //python脚本的路径
        //string path = @"F:\BUFFER\PycharmBuffer\" + sArgName; 
        string path = @"Assets/My/yolo-lite/" + sArgName;
        //string path = @"C:\WeConnect\PlugIn\PythonYolact-V1.1\" + sArgName;
        string sArguments = path;

        print("hello python");
        //(注意：用的话需要换成自己的)没有配环境变量的话，可以像我这样写python.exe的绝对路径
        //(用的话需要换成自己的)。如果配了，直接写"python.exe"即可
        p.StartInfo.FileName = @"C:\Users\TangQX\anaconda3\envs\yolo-unity\python.exe";
        //注意Ubuntu系统pyhton后缀没有.exe
        //p.StartInfo.FileName = @"C:\Program Files\Python35\python.exe";
        // sArguments为python脚本的路径   python值的传递路线strArr[]->teps->sigstr->sArguments 
        //在python中用sys.argv[ ]使用该参数
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.Arguments = sArguments;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardError = true;
        p.StartInfo.CreateNoWindow = true;
        p.Start();
        //print("1");
        //p.BeginOutputReadLine();
        //p.OutputDataReceived += new DataReceivedEventHandler(Out_RecvData);
        //Console.ReadLine();
        //p.WaitForExit();
        //print("3");
        return p;
    }

    public void KillProcess(Process p)
    {
        p.Kill();
    }

    static void Out_RecvData(object sender, DataReceivedEventArgs e)
    {
        print("2:--" + e.Data);
        if (!string.IsNullOrEmpty(e.Data))
        {
            print(e.Data);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        var stopWatch = new System.Diagnostics.Stopwatch();
        stopWatch.Start();
        yoloDetect = RunPythonScript(PythonName, "-u");
        //RunPythonScript(sArguments, "-u");

        stopWatch.Stop();

        var elapsed = stopWatch.Elapsed;
        Console.WriteLine(string.Format("{0}", elapsed));
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Alpha0))
        {
            yoloDetect.Kill();
        }
    }
}
