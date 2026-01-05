using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    public static NonPlayerCharacter Instance { get; internal set; }
    public GameObject UIDialogBox;
    private TextMeshProUGUI textMeshProUGUI;
    public float DisplayTime = 5f;
    private float displayTimer;

    /// <summary>
    /// 最大任务数量
    /// </summary>
    public int MaxQuestCount { get; set; }
    /// <summary>
    /// 当前任务完成数
    /// </summary>
    private int _currentCompleteCount;
    public int CurrentCompleteCount
    {
        get
        {
            return _currentCompleteCount;
        }
        set
        {
            _currentCompleteCount = value;

            textMeshProUGUI.text = $"Current task: {_currentCompleteCount} / {MaxQuestCount}";
            DisplayDialogue();
        }
    }

    /// <summary>
    /// 任务完成音效
    /// </summary>
    public AudioClip clipQuestComplete;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        textMeshProUGUI = UIDialogBox.GetComponentInChildren<TextMeshProUGUI>();
        UIDialogBox.SetActive(false);
        displayTimer = -1f;
    }

    void Update()
    {
        if (displayTimer >= 0)
        {
            displayTimer -= Time.deltaTime;
            if (displayTimer < 0)
            {
                UIDialogBox.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 显示对话框
    /// </summary>
    public void DisplayDialogue()
    {
        UIDialogBox.SetActive(true);
        displayTimer = DisplayTime;

        if (_currentCompleteCount >= MaxQuestCount)
        {
            AudioSource.PlayClipAtPoint(clipQuestComplete, Camera.main.transform.position);
            //audioSource.PlayOneShot(NonPlayerCharacter.Instance.clipQuestComplete);

            textMeshProUGUI.text = $"You did it!";
        }
    }

}
