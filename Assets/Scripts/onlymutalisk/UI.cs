using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;

public class UI : MonoBehaviour
{
    public TextMeshProUGUI Action_Text;
    public Image Action_Image;
    public GameObject Message;

    private void Update()
    {
        UpdateAction();
    }

    /// <summary>
    /// <br>캐릭터의 행동력에 따라 스프라이트를 변경합니다.</br>
    /// </summary>
    private void UpdateAction()
    {
        Action_Text.text = Player.action.ToString();

        if (Player.action <= 0) { Action_Image.sprite = Resources.Load<Sprite>("Images/Action_0"); }
        else if (Player.action < GameManager.action_Char * 0.25) { Action_Image.sprite = Resources.Load<Sprite>("Images/Action_1"); }
        else if (Player.action < GameManager.action_Char * 0.5) { Action_Image.sprite = Resources.Load<Sprite>("Images/Action_2"); }
        else if (Player.action < GameManager.action_Char * 0.75) { Action_Image.sprite = Resources.Load<Sprite>("Images/Action_3"); }
        else if (Player.action < GameManager.action_Char * 1) { Action_Image.sprite = Resources.Load<Sprite>("Images/Action_4"); }
        else { Action_Image.sprite = Resources.Load<Sprite>("Images/Action_5"); }
    }
}
