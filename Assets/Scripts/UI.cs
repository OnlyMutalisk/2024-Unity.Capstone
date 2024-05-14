using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public TextMeshProUGUI Action_Text;
    public Image Action_Image;
    public GameObject Message;
    private bool isTurnOn = true;

    private void Update()
    {
        UpdateAction();
    }

    /// <summary>
    /// <br>행동력에 따라 스프라이트를 변경하거나, 턴을 넘깁니다.</br>
    /// </summary>
    private void UpdateAction()
    {
        if (isTurnOn == true)
        {
            Action_Text.text = Player.action.ToString();

            if (Player.action <= 0)
            {
                Action_Image.sprite = Resources.Load<Sprite>("Images/Action_0");

                isTurnOn = false;
                TurnEnemy();
                isTurnOn = true;
            }
            else if (Player.action < GameManager.maxAction_Char * 0.25) { Action_Image.sprite = Resources.Load<Sprite>("Images/Action_1"); }
            else if (Player.action < GameManager.maxAction_Char * 0.5) { Action_Image.sprite = Resources.Load<Sprite>("Images/Action_2"); }
            else if (Player.action < GameManager.maxAction_Char * 0.75) { Action_Image.sprite = Resources.Load<Sprite>("Images/Action_3"); }
            else if (Player.action < GameManager.maxAction_Char * 1) { Action_Image.sprite = Resources.Load<Sprite>("Images/Action_4"); }
            else { Action_Image.sprite = Resources.Load<Sprite>("Images/Action_5"); }
        }
    }

    /// <summary>
    /// 적의 턴이 시작됩니다.
    /// </summary>
    private void TurnEnemy()
    {
        Message.SetActive(true);
    }
}
