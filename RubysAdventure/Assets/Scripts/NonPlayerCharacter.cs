using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    public GameObject UIDialogBox;
    public float DisplayTime = 5f;
    private float displayTimer;

    void Start()
    {
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
    /// œ‘ æ∂‘ª∞øÚ
    /// </summary>
    public void DisplayDialogue()
    {
        UIDialogBox.SetActive(true);
        displayTimer = DisplayTime;
    }

}
