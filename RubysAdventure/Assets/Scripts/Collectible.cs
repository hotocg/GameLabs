using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    /// <summary>
    /// 采集粒子效果
    /// </summary>
    public ParticleSystem CollectEffect;

    private void OnTriggerStay2D(Collider2D collision)
    {
        RubyController controller = collision.GetComponent<RubyController>();
        if (controller == null) return;

        // 获取自身标签
        var tag = gameObject.tag;
        if (tag == "Ammo")
        {
            if (controller.Ammo >= controller.MaxAmmo) return;
            // 增加弹药
            controller.ChangeAmmo(5);
        }
        else if (tag == "Health")
        {
            if (controller.Health >= controller.MaxHealth) return;
            // 增加生命
            controller.ChangeHealth(1);
        }

        // 创建粒子效果
        Instantiate(CollectEffect, transform.position, Quaternion.identity);
        // 销毁当前实例对象
        Destroy(gameObject);
    }
}
