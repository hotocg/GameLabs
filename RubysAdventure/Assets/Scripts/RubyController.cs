using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class RubyController : MonoBehaviour
{
    public float speed = 4f;
    private Rigidbody2D rb;
    private float horizontal;
    private float vertical;

    Animator animator;
    public bool isHorizontal;
    public bool isVertical;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 获取水平和垂直方向输入
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        //isHorizontal = horizontal != 0;
        //isVertical = vertical != 0;
        animator.SetFloat("MoveX", horizontal);
        animator.SetFloat("MoveY", vertical);

    }

    /// <summary>
    /// 固定更新，不受掉帧影响
    /// </summary>
    private void FixedUpdate()
    {
        var pos = rb.position;
        pos.x += horizontal * speed * Time.deltaTime;
        pos.y += vertical * speed * Time.deltaTime;
        rb.MovePosition(pos);
    }

}
