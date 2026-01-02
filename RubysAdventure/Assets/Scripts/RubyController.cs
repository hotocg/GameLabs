using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class RubyController : MonoBehaviour
{
    /// <summary>
    /// 最大生命值
    /// </summary>
    public int MaxHealth = 5;
    private int currentHealth;
    /// <summary>
    /// 当前生命值
    /// </summary>
    public int Health => currentHealth;

    /// <summary>
    /// 无敌时间（秒）
    /// </summary>
    public float InvincibleTime = 2f;
    /// <summary>
    /// 是否处于无敌状态
    /// </summary>
    private bool isInvincible = false;
    /// <summary>
    /// 无敌计时器（秒）
    /// </summary>
    private float invincibleTimer = 0;

    public float Speed = 4f;
    private Rigidbody2D rb;
    private float horizontal;
    private float vertical;

    Animator animator;

    /// <summary>
    /// 朝向
    /// </summary>
    Vector2 lookDirection = new Vector2(0, -1);

    /// <summary>
    /// 子弹
    /// </summary>
    public GameObject ProjectilePrefab;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = MaxHealth;
    }

    void Update()
    {
        // 获取水平和垂直方向输入
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        // 不接近 0 时（在移动有输入），设置朝向
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("LookX", lookDirection.x);
        animator.SetFloat("LookY", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
        }

    }

    /// <summary>
    /// 固定更新，不受掉帧影响
    /// </summary>
    private void FixedUpdate()
    {
        var pos = rb.position;
        pos.x += horizontal * Speed * Time.deltaTime;
        pos.y += vertical * Speed * Time.deltaTime;
        rb.MovePosition(pos);
    }

    /// <summary>
    /// 改变生命值
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible) return;
            isInvincible = true;
            invincibleTimer = InvincibleTime;
            animator.SetTrigger("Hit");
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, MaxHealth);
        Debug.Log($"{currentHealth} / {MaxHealth}");
    }

    /// <summary>
    /// 发射子弹
    /// </summary>
    void Launch()
    {
        // 创建子弹，因为角色轴心在底部，因此子弹往上偏移，视觉效果为从角色腰部手部的位置发射
        var newProjectile = Instantiate(ProjectilePrefab, rb.position + Vector2.up * 0.5f, Quaternion.identity);
        // 获取子弹脚本
        var projectile = newProjectile.GetComponent<Projectile>();
        // 发射
        projectile.Launch(lookDirection, 300f);
        // 播放角色发射动画
        animator.SetTrigger("Launch");

    }

}
