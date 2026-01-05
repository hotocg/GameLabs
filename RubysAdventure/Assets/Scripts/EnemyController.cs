using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float Speed = 5f;
    private Rigidbody2D rb;
    private AudioSource audioSource;

    public AudioClip clipRobotFixed;

    /// <summary>
    /// 是否垂直移动
    /// </summary>
    public bool IsVertical = false;
    /// <summary>
    /// 改变方向时间
    /// </summary>
    public float ChangeDirectionTime = 3f;
    /// <summary>
    /// 改变方向计时器
    /// </summary>
    private float directionTimer = 0f;
    /// <summary>
    /// 移动方向，1 或 -1
    /// </summary>
    private int direction = 1;

    private Animator animator;

    /// <summary>
    /// 是否损坏的
    /// </summary>
    private bool isBroken = true;

    /// <summary>
    /// 烟雾粒子效果
    /// </summary>
    public ParticleSystem SmokeEffect;
    /// <summary>
    /// 爆炸粒子效果
    /// </summary>
    public ParticleSystem BurstEffect;

    private void Awake()
    {

    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        NonPlayerCharacter.Instance.MaxQuestCount++;

    }

    void Update()
    {
        if (!isBroken) return;

        directionTimer -= Time.deltaTime;
        if (directionTimer < 0)
        {
            directionTimer = ChangeDirectionTime;
            direction = -direction;
        }

        if (IsVertical)
        {
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);
        }
        else
        {
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);
        }

    }

    private void FixedUpdate()
    {
        if (!isBroken) return;

        var pos = rb.position;
        if (IsVertical)
        {
            pos.y += direction * Speed * Time.deltaTime;
        }
        else
        {
            pos.x += direction * Speed * Time.deltaTime;
        }
        rb.MovePosition(pos);
    }

    /// <summary>
    /// 碰撞时触发
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        RubyController controller = collision.gameObject.GetComponent<RubyController>();
        if (controller != null)
        {
            controller.ChangeHealth(-1, gameObject.tag);
        }
    }

    /// <summary>
    /// 修复敌人
    /// </summary>
    public void Fix()
    {
        isBroken = false;

        animator.SetTrigger("Fixed");
        // 停止刚体模拟
        rb.simulated = false;
        // 停止烟雾粒子
        SmokeEffect.Stop();
        // 创建爆炸粒子
        Instantiate(BurstEffect, rb.position + Vector2.up * 0.5f, Quaternion.identity);
        // 播放音效
        audioSource.PlayOneShot(clipRobotFixed);

        NonPlayerCharacter.Instance.CurrentCompleteCount++;

        Destroy(gameObject, 2f);

    }


}
