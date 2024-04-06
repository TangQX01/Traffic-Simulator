using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectRebuilder : MonoBehaviour
{
    public int frame;
    public RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Rebuilder());// LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        //Debug.LogError("df");
    }

    // Update is called once per frame
    private IEnumerator Rebuilder()
    {
        yield return new WaitUntil(() => frame >= 10);
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    }
    void Update()
    {
        if (frame <= 10)
        {
            frame++;
        }
    }

}
