using FrameWorkSong;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWorkSongDemo
{
    public class HeatMapDemo : MonoBehaviour
    {
        public GameObject Object;
        public Transform Zero;
        public HeatMapBase heatMapBase;
        public HeatMapData[] Targets;
         void Start()
        {
            HeatMapCreat(Object, heatMapBase);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log(1);
                Debug.Log(heatMapBase.DisRatio);
                HeatMapCreat(Object, heatMapBase);
            }
        }
        public void HeatMapCreat(GameObject Object, HeatMapBase heatMapBase)
        {
        
            List<HeatMapInfo> heatMapInfos = new List<HeatMapInfo>();
            for (int i = 0; i < Targets.Length; i++)
            {
                Vector3 Pixel = (-Targets[i].Pos + Zero.position);
                HeatMapInfo heatMapInfo = new HeatMapInfo();
                heatMapInfo.Pixel = new Vector2(Pixel.x, Pixel.z);
                heatMapInfo.Amount = Targets[i].Amount;
                heatMapInfos.Add(heatMapInfo);
            }

            HeatMapBuild heatMapBuild = new HeatMapBuild(Object, heatMapBase, heatMapInfos);
            Object.GetComponent<Renderer>().material.mainTexture = heatMapBuild.GetHeatMapTexture();

        }

    }
}
