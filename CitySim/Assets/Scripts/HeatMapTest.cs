using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameWorkSongDemo;

namespace CitySim
{
    public class HeatMapTest : MonoBehaviour
    {
        //public GameObject m_CameraManager;
        //public List<Camera> m_CamList = new List<Camera>();
        public GameObject m_HeatmapCubes;
        [HideInInspector]
        public List<HeatMapData> m_Cubes = new List<HeatMapData>();
        public float m_TimePass = 0.0f;
        public float m_RanTime = 2.0f;
        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < m_HeatmapCubes.transform.childCount; i++)
            {
                HeatMapData cube = m_HeatmapCubes.transform.GetChild(i).GetComponent<HeatMapData>();
                if (cube)
                {
                    m_Cubes.Add(cube);
                    print(cube.name);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            m_TimePass += Time.deltaTime;
            if (m_TimePass >= m_RanTime)
            {
                m_RanTime = 3.0f * Random.value;
                m_TimePass = 0.0f;
                while (true)
                {
                    var index = Random.Range(0, m_Cubes.Count);
                    if (m_Cubes[index].Amount < 150)
                    {
                        m_Cubes[index].Amount += 30;
                        break;
                    }
                }
            }
        }
    }
}
