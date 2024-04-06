
using System;
using System.Collections.Generic;
using UnityEngine;


    [CreateAssetMenu(fileName = "Heat Map", menuName = "Templet/HeatMapBase", order = 1)]
    public class HeatMapBase : ScriptableObject
    {
        public float DisRatio=0.005f;
        public float Resolution=256;
        public List<HeatMapInfo> HeatMapInfos=new List<HeatMapInfo>();

        [Serializable]
        public sealed class HeatMapInfo
        {
            public float MaxAmount;
            public Color Color;
        }
    }

