using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public TextMeshProUGUI Action_Text;
    public Image Action_Image;

    private void Update()
    {
        UpdateAction();
    }

    private void UpdateAction()
    {
        Action_Text.text = Player.action.ToString();

        if (Player.action <= 0) { Action_Image.sprite = Resources.Load<Sprite>("Images/Action_0"); }
        else if (Player.action < GameManager.maxAction_Char * 0.25) { Action_Image.sprite = Resources.Load<Sprite>("Images/Action_1"); }
        else if (Player.action < GameManager.maxAction_Char * 0.5) { Action_Image.sprite = Resources.Load<Sprite>("Images/Action_2"); }
        else if (Player.action < GameManager.maxAction_Char * 0.75) { Action_Image.sprite = Resources.Load<Sprite>("Images/Action_3"); }
        else if (Player.action < GameManager.maxAction_Char * 1) { Action_Image.sprite = Resources.Load<Sprite>("Images/Action_4"); }
        else
        {
            Action_Image.sprite = Resources.Load<Sprite>("Images/Action_5");

            // 플레이어 턴 종료, 적의 턴 시작
        }
    }

    public void Attack()
    {
        // 적의 체력 깎기
    }
}
