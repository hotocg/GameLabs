using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        RubyController controller = collision.GetComponent<RubyController>();
        if (controller != null)
        {
            if (controller.Health >= controller.MaxHealth) return;

            // 增加生命
            controller.ChangeHealth(1);
            // 销毁当前实例对象
            Destroy(gameObject);
        }
    }
}
