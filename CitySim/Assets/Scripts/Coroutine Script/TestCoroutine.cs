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
        print("Print��ʼ��" + id + "ʮ������");
        
        //yield return new WaitForSeconds(10); //unity�е�ʮ�룬��Time.timeScale�й�
        yield return new WaitForSecondsRealtime(10); //��ʵ�����ʮ��
        //yield return null; //��һ֡��ʼ��ʱ��ִ�н�����������
        print("Print������" + id);
    }
}
