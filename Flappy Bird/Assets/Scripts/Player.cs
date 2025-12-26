using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region 位移
    ///// <summary>
    ///// 物理刚体组件的引用
    ///// </summary>
    //public Rigidbody2D rb;

    /// <summary>
    /// 移动方向
    /// </summary>
    private Vector3 direction;
    /// <summary>
    /// 重力（负）
    /// </summary>
    public float gravity = -9.8f;

    [Header("扇动力度")]
    [Tooltip("扇动力度")]
    [Range(1, 10)]
    public float flapStrength = 5f;

    /// <summary>
    /// 屏幕顶部
    /// </summary>
    private float screenTop;
    /// <summary>
    /// 屏幕底部
    /// </summary>
    private float screenBottom;
    /// <summary>
    /// 地面
    /// </summary>
    [Header("地面设置")]
    public MeshRenderer groundRenderer;
    private float groundTopY;

    [Tooltip("旋转倍率")]
    public float tiltStrength = 5f;
    #endregion

    #region 动画

    [Header("动画")]
    /// <summary>
    /// 动画组件的引用
    /// </summary>
    private Animator anim;

    /// <summary>
    /// 2D 图片组件的引用
    /// </summary>
    private SpriteRenderer spriteRenderer;
    /// <summary>
    /// 小鸟动画帧数组
    /// </summary>
    public Sprite[] sprites;
    /// <summary>
    /// 小鸟动画帧当前索引
    /// </summary>
    private int index = 0;
    #endregion

    /// <summary>
    /// 脚本实例被加载时
    /// </summary>
    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 在第一次帧更新之前调用
    /// </summary>
    void Start()
    {
        // 获取主摄像机的垂直范围（单位是 Unity 单位）
        screenTop = Camera.main.orthographicSize;
        screenBottom = -Camera.main.orthographicSize;

        if (groundRenderer != null)
        {
            // groundMesh.bounds.center.y 是 Quad 的中心点 Y
            // groundMesh.bounds.extents.y 是中心点到边缘的距离（即高度的一半）
            groundTopY = groundRenderer.bounds.center.y + groundRenderer.bounds.extents.y;

            var birdHalfHeight = spriteRenderer.bounds.extents.y / 2f;
            groundTopY += birdHalfHeight;
        }

        // ========== 动画
        // 方法1
        if (anim == null) Debug.LogError("在对象 " + gameObject.name + " 上没找到 Animator 组件");

        // 方法2
        //// 重复调用函数：每 0.15 秒换一帧
        //InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);

    }

    /// <summary>
    /// 更新操作每帧调用一次
    /// </summary>
    void Update()
    {
        HandleInput();
        ApplyMovement();
        UpdateVisuals();

    }


    private void HandleInput()
    {
        // 空格 或者 鼠标左键
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || IsTouchBegan())
        {
            // 每次点击都重置初始值
            direction = Vector3.up * flapStrength;

            // 给小鸟一个向上的瞬时速度
            // Vector2.up 等于 (0, 1)，乘以力量后变成 (0, 5)
            //rb.velocity = Vector2.up * flapStrength;
        }
    }

    /// <summary>
    /// 应用位移
    /// </summary>
    private void ApplyMovement()
    {
        // 按帧间隔时长
        direction.y += gravity * Time.deltaTime; // 速度 = 初始速度 + 加速度 * 时间
        transform.position += direction * Time.deltaTime; // 位移 = 位移 + 速度 * 时间

        // 边界限制，避免超出屏幕
        screenBottom = groundTopY; // 地面半高 + 小鸟半高

        // 使用 Mathf.Clamp 限制 y 轴范围：如果 position.y 超过上限，就取上限；低于下限，就取下限
        float clampedY = Mathf.Clamp(transform.position.y, screenBottom, screenTop);
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);

        // 优化手感，如果撞到了天花板，应该把向上速度归零，否则小鸟会滞留一会
        if (transform.position.y >= screenTop && direction.y > 0) direction.y = 0;
    }

    /// <summary>
    /// 更新动画
    /// </summary>
    //void AnimateSprite()
    //{
    //    index++;
    //    if (index >= sprites.Length) index = 0;
    //    spriteRenderer.sprite = sprites[index];
    //}

    /// <summary>
    /// 更新视觉效果
    /// </summary>
    private void UpdateVisuals()
    {
        // 1. 翅膀动画
        if (anim != null)
        {
            // 如果方向向上（在飞），播放速度为 1；如果掉落，播放速度为 0（定格）
            anim.speed = (direction.y > 0) ? 1f : 0f;
        }

        // 2. 旋转逻辑，让小鸟的头随速度旋转，速度向上时，头向上抬；速度向下时，头向下低
        float tilt = direction.y * tiltStrength; // 根据 y 轴速度计算旋转角度
        tilt = Mathf.Clamp(tilt, -90f, 30f); // 限制旋转范围（抬头最高 30 度，低头最低 -90 度）

        // 落地
        if (transform.position.y <= screenBottom && direction.y < 0)
        {
            tilt = 0f;
        }

        transform.rotation = Quaternion.Euler(0, 0, tilt);
    }

    private void OnEnable()
    {
        // 重新开始游戏后，重置位置
        direction.y = 0f;
        transform.position = Vector3.zero;
    }

    /// <summary>
    /// 判断是否触摸开始
    /// </summary>
    /// <returns></returns>
    private bool IsTouchBegan()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var tag = collision.gameObject.tag;
        //Debug.Log("碰撞了：" + tag);

        switch (tag)
        {
            case "Obstacle":
                FindObjectOfType<GameManager>().GameOver();
                break;
            case "Scoring":
                FindObjectOfType<GameManager>().AddScore();
                break;
        }


    }

}
