using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public static UIHealthBar Instance { get; private set; }

    public Image Mask;
    private float originalSize;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // 获取原始宽度
        originalSize = Mask.rectTransform.rect.width;
    }

    /// <summary>
    /// 设置血条宽度
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(float value)
    {
        // 设置当前锚点的尺寸
        Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }
}
