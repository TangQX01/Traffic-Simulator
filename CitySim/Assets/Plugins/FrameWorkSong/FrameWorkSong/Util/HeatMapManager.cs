using FrameWorkSong;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;

namespace FrameWorkSongDemo
{
    public class HeatMapManager : MonoBehaviour
    {
        public GameObject Object;
        public Transform Zero;
        public HeatMapBase heatMapBase;
        public GameObject HeatMapCube;
        private HeatMapData[] Targets
        {
            get
            {
                HeatMapData[] res = new HeatMapData[HeatMapCube.transform.childCount];
                //print(HeatMapCube.name);
                for (int i = 0; i < HeatMapCube.transform.childCount; i++)
                {
                    if (HeatMapCube.transform.GetChild(i).GetComponent<HeatMapData>() == null)
                    {
                        HeatMapCube.transform.GetChild(i).gameObject.AddComponent(typeof(HeatMapData));
                    }
                    res[i] = HeatMapCube.transform.GetChild(i).GetComponent<HeatMapData>();
                    //res[i].Amount = 0;
                    //print((res[i].Amount));
                }
                return res;
            }
        }


        void Start()
        {
            StartCoroutine(WaitHeatMapCreat());
        }
        private void Update()
        {
           
        }
        public IEnumerator WaitHeatMapCreat()
        {
            while (true)
            {
                HeatMapCreat(Object, heatMapBase);
                yield return new WaitForSeconds(1);
            }
        }

        private void HeatMapCreat(GameObject Object, HeatMapBase heatMapBase)
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
