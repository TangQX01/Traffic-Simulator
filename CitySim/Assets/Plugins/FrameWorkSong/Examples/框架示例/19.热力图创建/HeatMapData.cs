using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWorkSongDemo
{
    public class HeatMapData : MonoBehaviour
    {
        private Vector3 pos;
        [Range(0, 150)]
        public int Amount;

        public Vector3 Pos { get { return transform.position; } set { pos = value; } }
    }
}
