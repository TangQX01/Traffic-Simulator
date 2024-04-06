//==============================================================================================
//Author: Uncle Song
//Create Date: 2022-06-20
//Description: 热力图数据
//----------------------------------------------------------------------------------------------
//Alter History:
//              2022-06-20  add Codes.
//============================================================================================== 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWorkSong
{
    public class HeatMapInfo
    {
        Vector2 pixel;
        int amount;//数量

        public Vector2 Pixel { get => pixel; set => pixel = value; }
        public int Amount { get => amount; set => amount = value; }
    }
   


    public class HeatMapBuild
    {
        Texture2D HeatMapTexture;
        /// <summary>
        /// 构造HeatMapBuild
        /// </summary>
        /// <param name="gameObject">创建热力图的载体</param>
        /// <param name="heatMapBase">热力图模板</param>
        /// <param name="heatMapInfos">热力图数据</param>
        public HeatMapBuild(GameObject gameObject, HeatMapBase heatMapBase, List<HeatMapInfo> heatMapInfos)
        {
            Vector3 modelSize = gameObject.GetComponent<Collider>().bounds.size;
            float MapRatio = heatMapBase.Resolution / modelSize.x;

            HeatMapTexture = new Texture2D((int)heatMapBase.Resolution, (int)heatMapBase.Resolution);

            CreatHeatMap(ref HeatMapTexture, heatMapInfos, heatMapBase, MapRatio);
        }
        public Texture2D GetHeatMapTexture()
        {
            if (HeatMapTexture)
            {
                return HeatMapTexture;
            }
            else
            {
                return Texture2D.whiteTexture;
            }
        }

        void CreatHeatMap(ref Texture2D texture, List<HeatMapInfo> heatMapInfos, HeatMapBase heatMapBase, float MapRatio)
        {
            //每个像素初始化
            List<Vector2> my_Pixels = new List<Vector2>();
            List<float> my_Values = new List<float>();
            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {

                    Vector2 pixel = new Vector2(x, y);
                    my_Pixels.Add(pixel);
                    my_Values.Add(0);
                }
            }
            int allLength = my_Pixels.Count;//所以像素点数量
            int pointLength = heatMapInfos.Count;//热力图兴趣点点数量
            int colorLength = heatMapBase.HeatMapInfos.Count;//色块等级
            //每个像素加权值
            for (int p = 0; p < pointLength; p++)
            {
                for (int a = 0; a < allLength; a++)
                {
                    float my_Distance = Vector2.Distance(heatMapInfos[p].Pixel* MapRatio, my_Pixels[a]);

                    float my_MaxDis = heatMapBase.DisRatio * heatMapInfos[p].Amount;

                    if (my_Distance < my_MaxDis)
                    {

                        float value = (1 - (Mathf.Pow(my_Distance, 2) / Mathf.Pow(my_MaxDis, 2))) * heatMapInfos[p].Amount;
                        if (value > my_Values[a])
                        {
                            my_Values[a] = value;
                        }
                    }
                }
            }
            //每个像素赋值颜色
            for (int i = 0; i < allLength; i++)
            {
                if (i <= heatMapBase.Resolution || i >= allLength- heatMapBase.Resolution|| i% heatMapBase.Resolution==0|| (i-1) % heatMapBase.Resolution == 0)
                {
                    texture.SetPixel((int)my_Pixels[i].x, (int)my_Pixels[i].y, Color.clear);
                    continue;
                }
              

                for (int j = 0; j < colorLength; j++)
                {
                    float my_CurMaxAmount = heatMapBase.HeatMapInfos[j].MaxAmount;
                  
                    if (my_Values[i] >= my_CurMaxAmount)
                    {
                        //当前块的颜色
                        Color my_CurColor = heatMapBase.HeatMapInfos[j].Color;

                        if (j != 0)
                        {

                            float my_UpDiffValue = heatMapBase.HeatMapInfos[j - 1].MaxAmount - my_CurMaxAmount;
                            Color my_UpColor = heatMapBase.HeatMapInfos[j - 1].Color;

                            float t = (my_Values[i] - my_CurMaxAmount) / my_UpDiffValue;
                            texture.SetPixel((int)my_Pixels[i].x, (int)my_Pixels[i].y, Color.Lerp(my_CurColor, my_UpColor, t));
                            break;
                        }
                        else
                        {
                            texture.SetPixel((int)my_Pixels[i].x, (int)my_Pixels[i].y, my_CurColor);
                            break;
                        }
                    }
                }
            }
            texture.Apply();
        }
    }
}
