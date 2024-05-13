using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    private int index;
    private int i, j;
    public Vector3 pos;
    public static int cost;

    public void Start()
    {
        // gameObject.name 으로부터 index 추출
        Match match = Regex.Match(gameObject.name, @"\d+");
        if (match.Success) { index = int.Parse(match.Value); }
        else { index = 0; }

        // index 으로부터 i, j 추출
        (i, j) = Grid.ConvertIndexToArray(index);

        // 월드 좌표 저장
        pos.x = gameObject.GetComponent<RectTransform>().rect.position.x;
        pos.y = gameObject.GetComponent<RectTransform>().rect.position.y;
    }

    public void CheckAction()
    {
        if (GameManager.playerAction == GameManager.PlayerAction.Move)
        {
            // Player.Move();
        }
        if (GameManager.playerAction == GameManager.PlayerAction.Attack)
        {
            // Player.Attack();
        }
        if (GameManager.playerAction == GameManager.PlayerAction.Skill)
        {
            // Player.Skill();
        }
    }

    /// <summary>
    /// <br>타일이 Select 상태라면, 이동합니다.</br>
    /// </summary>
    public void OnClick()
    {
        Debug.Log(Grid.GetTile(i, j).name + " = [" + i + "][" + j + "]");

        if (gameObject.GetComponent<Image>().sprite.name == "Tile_Select")
        {
            // 비숍 이동 비용 계산
            if (Math.Abs(Player.i - this.i) == Math.Abs(Player.j - this.j))
            {
                cost = Math.Abs(Player.i - i) * cost;
            }

            // 룩 이동 비용 계산
            if ((Math.Abs(Player.i - this.i) > 0 && Math.Abs(Player.j - this.j) == 0) || (Math.Abs(Player.j - this.j) > 0 && Math.Abs(Player.i - this.i) == 0))
            {

                cost = (Math.Abs(Player.i - i) + Math.Abs(Player.j - j)) * cost;
            }

            Player.action -= cost;

            StartCoroutine(Player.CorMove(i, j));
        }
    }
}