
using UnityEngine;
using System.Collections.Generic;

public enum HeatMapMode
{
    RefreshEachFrame,//每帧更新
    RefreshByInterval//定时更新
}

public class HeatMapComponent : MonoBehaviour
{
    private Material m_material = null;

    public Material material
    {
        get
        {
            if (null == m_material)
            {
                var render = this.GetComponent<Renderer>();
                m_material = render.material;
            }
            return m_material;
        }
    }

    public HeatMapMode heatMapMode = HeatMapMode.RefreshEachFrame;

    public float interval = 0.02f;
    private float m_timer = 0.0f;

    public List<HeatMapFactor> impactFactors = new List<HeatMapFactor>();

    private void Update()
    {
        if (heatMapMode == HeatMapMode.RefreshEachFrame)
        {
            RefreshHeatmap();
            return;
        }
        m_timer += Time.deltaTime;
        if (m_timer > interval)
        {
            RefreshHeatmap();
            m_timer -= interval;
        }
    }

    private void RefreshHeatmap()
    {
        material.SetInt("_FactorCount", impactFactors.Count);

        var ifPosition = new Vector4[impactFactors.Count];
        for (int i = 0; i < impactFactors.Count; i++)
            ifPosition[i] = impactFactors[i].transform.position;
        material.SetVectorArray("_Factors", ifPosition);

        var properties = new Vector4[impactFactors.Count];
        for (int i = 0; i < impactFactors.Count; i++)
        {
            var factor = impactFactors[i];
            properties[i] = new Vector4(factor.influenceRadius, factor.intensity, factor.temperatureFactor, 0.0f);
        }
        material.SetVectorArray("_FactorsProperties", properties);

    }
}
