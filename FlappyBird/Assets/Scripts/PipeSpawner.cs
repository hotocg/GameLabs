using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    /// <summary>
    /// 管道预制体引用
    /// </summary>
    public GameObject pipePrefab;
    /// <summary>
    /// 生成管道的间隔时间
    /// </summary>
    public float spawnRate = 1f;
    /// <summary>
    /// 管道生成时Y轴的最小值
    /// </summary>
    public float minPipeY = -1f;
    /// <summary>
    /// 管道生成时Y轴的最大值
    /// </summary>
    public float maxPipeY = 2f;

    void OnEnable()
    {
        // 定时生成管道
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }
    
    void OnDisable()
    {
        // 停止生成（取消此行为上所有名为 methodName 的 Invoke 调用）
        CancelInvoke(nameof(Spawn));
    }

    /// <summary>
    /// 生成管道
    /// </summary>
    void Spawn()
    {
        var pos = new Vector3(transform.position.x, Random.Range(minPipeY, maxPipeY), 0);
        Instantiate(pipePrefab, pos, Quaternion.identity);
    }

}
