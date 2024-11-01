using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    private int index;
    private GameObject msg_top;
    private TextMeshProUGUI tmp;
    public int i, j;
    public int F; // 목적지까지의 경로 총 비용
    public int G; // 시작점 to 경유지 비용
    public int H; // 경유지 to 목적지 비용
    public int parentsCount; // 모든 부모 경로 타일 수
    public Tile parentsTile; // 부모 경로 타일
    public GameObject meteor;
    public Vector3 pos;
    public bool isWall = false;
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

        // Message_Top 연결
        msg_top = Message_Move.FindChildObject(GameObject.Find("UI"), "Message_Top");

        tmp = msg_top.transform.Find("Location").Find("Text").GetComponent<TextMeshProUGUI>();
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

            if (Player.action >= cost)
            {
                Player.action -= cost;

                StartCoroutine(Player.CorMove(i, j));
            }
            else
            {
                StartCoroutine(OnMsgTop(GameManager.msg_action, GameManager.delay_msgTopAction));
            }
        }
    }

    private IEnumerator OnMsgTop(string msg, float seconds)
    {
        if (msg_top.active == false)
        {
            msg_top.SetActive(true);
            tmp.text = msg;
            yield return new WaitForSeconds(seconds);
            msg_top.SetActive(false);
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
                    for (int n = 0; n < Tile.tiles.Count; n++) { Tile.tiles[n].sprite = Tile.origins[n]; }
                    Tile.tiles.Clear();
                    Tile.origins.Clear();
                    Tile.isTileOn = false;

                    if (Player.action >= GameManager.cost_Attack)
                    {
                        Player.action -= GameManager.cost_Attack;
                        mob.HP -= CalcDamage(GameManager.attackDamage_Char, Grid.GetTile(Player.i, Player.j), Grid.GetTile(this.i, this.j));
                        if (mob.HP <= 0) { KillMob(mob); }
                        break;
                    }
                    else
                    {
                        StartCoroutine(OnMsgTop(GameManager.msg_action, GameManager.delay_msgTopAction));
                    }
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
                    for (int n = 0; n < Tile.tiles.Count; n++) { Tile.tiles[n].sprite = Tile.origins[n]; }
                    Tile.tiles.Clear();
                    Tile.origins.Clear();
                    Tile.isTileOn = false;

                    if (Player.action >= GameManager.cost_Skill)
                    {
                        StartCoroutine(CorMeteor());
                        Player.action -= GameManager.cost_Skill;
                        mob.HP -= GameManager.skillDamage_Char;
                        mob.isSleep = false;
                        if (mob.HP <= 0) { KillMob(mob); }
                        break;
                    }
                    else
                    {
                        StartCoroutine(OnMsgTop(GameManager.msg_action, GameManager.delay_msgTopAction));
                    }
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

    /// <summary>
    /// <br>Contains 메서드에서 i 값과 j 값을 기준으로 판단하도록 Equals 를 재정의합니다.</br>
    /// </summary>
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType()) { return false; }
        else
        {
            Tile other = (Tile)obj;
            return i == other.i && j == other.j;
        }
    }

    /// <summary>
    /// <br>Equals 를 재정의 할 때, 해시 코드도 재정의 해야합니다.</br>
    /// <br>이는 해시 기반 컬렉션에서도 올바르게 탐색할 수 있도록 합니다.</br>
    /// </summary>
    public override int GetHashCode()
    {
        return (i, j).GetHashCode();
    }

    /// <summary>
    /// <br>타일의 이미지를 전역 리스트에 추가합니다.</br>
    /// <br>두 번째 파라미터로 true 를 넣으면 조건에 상관없이 리스트에 추가합니다.</br>
    /// </summary>
    public static void AddTileImages(GameObject tile, bool checkMob = false)
    {
        if (checkMob == false)
        {
            if (tile.GetComponent<Tile>().isWall == false)
            {
                tiles.Add(tile.GetComponent<Image>());
            }
        }
        else
        {
            tiles.Add(tile.GetComponent<Image>());
        }
    }

    /// <summary>
    /// 타일 속성에 따라 데미지에 가중치를 부여합니다.
    /// </summary>
    public static float CalcDamage(float damage, GameObject attacker, GameObject target)
    {
        Tile attackerTile = attacker.GetComponent<Tile>();
        Tile targetTile = target.GetComponent<Tile>();

        string property_attacker = attackerTile.gameObject.GetComponent<Image>().sprite.name;
        string property_target = targetTile.gameObject.GetComponent<Image>().sprite.name;

        foreach (KeyValuePair<string, string> property in GameManager.propertyCounterMatch)
        {
            if (property_attacker == property.Key && property_target == property.Value)
            {
                damage *= GameManager.propertyBonus;
            }
        }

        return damage;
    }

    /// <summary>
    /// 몬스터를 제거합니다.
    /// </summary>
    private void KillMob(Mob mob)
    {
        Grid.GetTile(mob.i, mob.j).GetComponent<Tile>().isWall = false;
        Mob.Mobs.Remove(mob);
        Destroy(mob.gameObject);
    }
}