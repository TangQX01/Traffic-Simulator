using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCoroutine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(WaitAndPrint(0));
        //StartCoroutine(WaitAndPrint(1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator WaitAndPrint(int id)
    {
        print("Print开始：" + id + "十秒后结束");
        
        //yield return new WaitForSeconds(10); //unity中的十秒，与Time.timeScale有关
        yield return new WaitForSecondsRealtime(10); //真实世界的十秒
        //yield return null; //下一帧开始的时候执行接下来的任务
        print("Print结束：" + id);
    }
}
