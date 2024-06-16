using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Tile : MonoBehaviour
{
    private int index;
    private int i, j;
    public GameObject meteor;
    public Vector3 pos;
    public bool isWall;
    public static int cost;
    public static List<Image> tiles = new List<Image>();
    public static List<Sprite> origins = new List<Sprite>();
    public static bool isTileOn = false;

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

    /// <summary>
    /// <br>타일이 Select 상태라면, 이동합니다.</br>
    /// </summary>
    public void OnClick()
    {
        if (GameManager.playerAction == GameManager.PlayerAction.Move) { Move(); }
        if (GameManager.playerAction == GameManager.PlayerAction.Attack) { Attack(); }
        if (GameManager.playerAction == GameManager.PlayerAction.Skill) { Skill(); }
        if (GameManager.playerAction == GameManager.PlayerAction.Skill)
        {
            // Player.Skill();
        }
    }

    /// <summary>
    /// <br>Tile_Select 타일 터치 시, 해당 타일로 플레이어를 이동시킵니다.</br>
    /// </summary>
    private void Move()
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

    /// <summary>
    /// Tile_Select_Attack 타일 터치 시, 공격 범위 내 적 유닛에게 데미지를 입힙니다.
    /// </summary>
    public void Attack()
    {
        if (gameObject.GetComponent<Image>().sprite.name == "Tile_Select_Attack")
        {
            foreach (var mob in Mob.Mobs)
            {
                // 터치한 타일에 적이 존재한다면 공격
                if (mob.i == this.i && mob.j == this.j)
                {
                    Player.action -= GameManager.cost_Attack;
                    mob.HP -= GameManager.damage_Char;

                    for (int n = 0; n < Tile.tiles.Count; n++) { Tile.tiles[n].sprite = Tile.origins[n]; }
                    Tile.tiles.Clear();
                    Tile.origins.Clear();
                    Tile.isTileOn = false;
                }
            }
        }
    }

    /// <summary>
    /// Tile_Select_Skill 타일 터치 시, 스킬 범위 내 적 유닛에게 데미지를 입힙니다.
    /// </summary>
    public void Skill()
    {
        if (gameObject.GetComponent<Image>().sprite.name == "Tile_Select_Skill")
        {
            foreach (var mob in Mob.Mobs)
            {
                // 터치한 타일에 적이 존재한다면 공격
                if (mob.i == this.i && mob.j == this.j)
                {
                    StartCoroutine(CorMeteor());

                    Player.action -= GameManager.cost_Skill;
                    mob.HP -= GameManager.damage_Char;

                    for (int n = 0; n < Tile.tiles.Count; n++) { Tile.tiles[n].sprite = Tile.origins[n]; }
                    Tile.tiles.Clear();
                    Tile.origins.Clear();
                    Tile.isTileOn = false;
                }
            }
        }
    }

    public IEnumerator CorMeteor()
    {
        Vector3 meteorPos = transform.position;
        meteorPos.x += 3 * Grid.cellSize;
        meteorPos.y += 3 * Grid.cellSize;
        GameObject meteor = Instantiate(this.meteor, meteorPos, Quaternion.identity);

        while (Vector3.Distance(meteor.transform.position, transform.position) > 0.1f)
        {
            meteor.transform.position = Vector3.MoveTowards(meteor.transform.position, transform.position, 300 * Time.deltaTime);
            yield return null;
        }

        Destroy(meteor);
    }
}