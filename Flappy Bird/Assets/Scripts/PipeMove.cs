using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float leftBound = 0f;

    void Start()
    {
        // 获取视口左侧边界
        //var pipeWidth = 2f;
        var pipeWidth = GetComponentInChildren<SpriteRenderer>().bounds.size.x; // 管道宽度，在预制件的子对象中获取
        leftBound = Camera.main.ScreenToWorldPoint(Vector3.zero).x - pipeWidth;
    }

    void Update()
    {
        // 移动管道
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        // 如果离开视口，销毁管道
        if (transform.position.x < leftBound)
        {
            Destroy(gameObject);
        }
    }
}
