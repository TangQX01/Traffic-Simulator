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
                print("还没按下Esc");
                //yield return new WaitForSeconds(10); //unity中的十秒，与Time.timeScale有关
                //yield return new WaitForSecondsRealtime(10); //真实世界的十秒
                //yield return null; //下一帧开始的时候执行接下来的任务
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Escape));
                print("刚刚按下Esc");
            }
        }
        public IEnumerator WaitAndPrint(int id)
        {
            print("Print开始：" + id + "十秒后结束");

            //yield return new WaitForSeconds(10); //unity中的十秒，与Time.timeScale有关
            yield return new WaitForSecondsRealtime(10); //真实世界的十秒
            //yield return null; //下一帧开始的时候执行接下来的任务
            print("Print结束：" + id);
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