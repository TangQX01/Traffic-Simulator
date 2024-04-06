using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CitySim {
    partial class CarManager
    {
        void TestPartial()
        {
            print("123");
            print(m_CamList.Count);
        }

        public IEnumerator TestCoroutine()
        {
            while (true)
            {
                print("��û����Esc");
                //yield return new WaitForSeconds(10); //unity�е�ʮ�룬��Time.timeScale�й�
                //yield return new WaitForSecondsRealtime(10); //��ʵ�����ʮ��
                //yield return null; //��һ֡��ʼ��ʱ��ִ�н�����������
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Escape));
                print("�ոհ���Esc");
            }
        }
        public IEnumerator WaitAndPrint(int id)
        {
            print("Print��ʼ��" + id + "ʮ������");

            //yield return new WaitForSeconds(10); //unity�е�ʮ�룬��Time.timeScale�й�
            yield return new WaitForSecondsRealtime(10); //��ʵ�����ʮ��
            //yield return null; //��һ֡��ʼ��ʱ��ִ�н�����������
            print("Print������" + id);
        }

        public IEnumerator WaitHOIDetection(Camera camera)
        {
            while (true)
            {
                m_Socket.screenShoot(camera);
                yield return new WaitForSeconds(10);
            }
        }
    }

}