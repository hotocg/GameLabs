using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float speed = 0.05f;
    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // 计算偏移量：随时间增加而增加
        var offset = new Vector2(Time.time * speed, 0);
        // 设置材质纹理偏移
        meshRenderer.material.mainTextureOffset = offset;
    }
}
