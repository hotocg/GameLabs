using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// 发射子弹
    /// </summary>
    /// <param name="direction">方向</param>
    /// <param name="force">力度</param>
    public void Launch(Vector2 direction, float force)
    {
        rb.AddForce(direction * force);
    }

    private void Update()
    {
        //Debug.Log($"{transform.position.ToString()} | {transform.position.magnitude} {transform.position.magnitude > 100f}");
        // 从世界中心到对象所在位置的向量，如果该向量的长度超过指定值，则销毁子弹
        if (transform.position.magnitude > 100f)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 击中目标
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"击中 {collision.gameObject.name}");
        var enemy = collision.collider.GetComponent<EnemyController>();
        enemy?.Fix();
        Destroy(gameObject); // 销毁子弹
    }

}
