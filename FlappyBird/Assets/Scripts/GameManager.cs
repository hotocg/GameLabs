using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverImage;
    public GameObject playButton;
    public AudioSource scoreSound;

    /// <summary>
    /// 当前评分
    /// </summary>
    private int scoring = 0;

    private void Awake()
    {
        Pause();
    }

    public void Pause()
    {
        // 全局时间流逝速度设为0，暂停游戏（所有物体都会停止）
        Time.timeScale = 0f;
        player.enabled = false;
    }

    public void OnPlayButtonClick()
    {
        scoring = 0;
        scoreText.text = scoring.ToString();

        Time.timeScale = 1f;

        player.enabled = true;
        gameOverImage.SetActive(false);
        playButton.SetActive(false);

        // 销毁所有管道
        var Pipes = FindObjectsOfType<PipeMove>();
        for (int i = 0; i < Pipes.Length; i++)
        {
            Destroy(Pipes[i].gameObject);
        }

    }

    public void GameOver()
    {
        gameOverImage.SetActive(true);
        playButton.SetActive(true);
        Pause();
    }

    public void AddScore()
    {
        scoring++;
        scoreText.text = scoring.ToString();
        scoreSound.PlayOneShot(scoreSound.clip);
    }

}
