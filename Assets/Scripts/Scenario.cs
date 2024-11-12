using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Scenario : MonoBehaviour
{
    public GameObject message_top;
    public TextMeshProUGUI text;
    public GameObject turn;
    public GameObject inventory;
    public GameObject vision;
    public GameObject attack;
    public GameObject action;
    public GameObject skill;
    public GameObject pawn;
    public GameObject knight;
    public GameObject bishop;
    public GameObject rook;
    public GameObject mark;

    private bool map0_pawn;


    /// <summary>
    /// 시나리오 데몬입니다.
    /// </summary>
    private void Start()
    {
        StartCoroutine(CorStart());
    }
    public IEnumerator CorStart()
    {
        // 로딩 대기시간
        yield return new WaitForSeconds(GameManager.delay_loading);

        switch (Map.index)
        {
            case 0:
                turn.SetActive(false);
                inventory.SetActive(false);
                vision.SetActive(false);
                knight.SetActive(false);
                bishop.SetActive(false);
                rook.SetActive(false);
                OnMsg("Pawn 을 클릭해 전진하세요!", 999f);
                OnMark(pawn, 999f);

                // Pawn 클릭할 때 까지 STOP
                while (Grid.GetTile(Player.i - 1, Player.j).GetComponent<Image>().sprite.name != "Tile_Select") yield return new WaitForSeconds(0.5f);
                OnMsg("하늘색 땅을 클릭해 전진하세요!", 999f);
                OnMark(Grid.GetTile(Player.i - 1, Player.j), 999);

                // 전진할 때 까지 STOP
                while (Player.action == Player.maxAction) yield return new WaitForSeconds(0.5f);
                OnMsg("앞으로 계속 나아가\n적을 마주하세요!", 999f);
                OffMark();

                // 몬스터와 조우할 때 까지 STOP
                bool b1 = true;
                GameObject enemy = new GameObject();
                while (b1)
                {
                    foreach (Mob mob in Mob.Mobs)
                    {
                        if (A_Star.GetDistance(Grid.GetTile(Player.i, Player.j).GetComponent<Tile>(), Grid.GetTile(mob.i, mob.j).GetComponent<Tile>(), RangeType.Manhattan) == 1)
                        {
                            b1 = false;
                            enemy = mob.gameObject;
                            break;
                        }
                    }

                    yield return new WaitForSeconds(0.5f);
                }
                OnMsg("공격 버튼을 클릭하세요 !", 999f);
                OnMark(attack, 999f);

                // 공격 버튼을 누를 때 까지 STOP
                while (Grid.GetTile(Player.i - 1, Player.j).GetComponent<Image>().sprite.name != "Tile_Select_Attack") yield return new WaitForSeconds(0.5f);
                OnMsg("적을 무찌르세요 !", 999f);
                OnMark(enemy, 999f);
                int maxMobCount = Mob.Mobs.Count;

                // 한 마리를 처치할 때 까지 STOP
                while (maxMobCount == Mob.Mobs.Count) yield return new WaitForSeconds(0.5f);
                OnMsg("모든 적을 무찌르고\n스테이지를 클리어하세요.", 999f);
                OffMark();
                Player.life = GameManager.Life_Char;
                Mob.DrawLife();

                break;
            case 1:
                inventory.SetActive(false);
                vision.SetActive(false);
                bishop.SetActive(false);
                rook.SetActive(false);
                OnMsg("적을 찾아 무찌르세요 !", 999f);
                int start_i = Player.i;

                // 두 칸 전진할 때 까지 STOP
                while (Player.i != start_i - 2) yield return new WaitForSeconds(0.5f);
                OnMsg("Knight 로 장애물을 넘어가세요 !", 999f);
                OnMark(knight, 999f);

                // 나이트를 사용할 때 까지 STOP
                while (Player.action > Player.maxAction - (GameManager.cost_Pawn * 2 + GameManager.cost_Knight)) yield return new WaitForSeconds(0.5f);
                OnMsg("좋습니다! 계속 전진하세요.", 999f);
                OffMark();

                // 행동력이 1 이 될 때 까지 STOP
                while (Player.action != 1) yield return new WaitForSeconds(0.5f);
                OnMsg("행동력이 부족합니다.\n턴을 종료하세요.", 999f);
                OnMark(action, 999f);

                // 턴 종료할 때 까지 STOP
                while (Player.action != Player.maxAction) yield return new WaitForSeconds(0.5f);
                OnMsg("남은 턴 수 내에 클리어하지 못하면 패배합니다. 서두르세요 !", 999f);
                Vector2 pos = message_top.transform.position;
                message_top.transform.position = new Vector2(pos.x, pos.y - 25);
                OnMark(turn, 10f);

                // 적 조우 시 STOP
                while (A_Star.GetDistance(Grid.GetTile(Player.i, Player.j).GetComponent<Tile>(), Grid.GetTile(Mob.Mobs[0].i, Mob.Mobs[0].j).GetComponent<Tile>(), RangeType.Manhattan) > 4) yield return new WaitForSeconds(0.5f);
                OnMsg("조심하세요 ! 적이 Knight 처럼 움직이며\n원거리 공격을 사용합니다.", 10f);
                OffMark();
                pos = message_top.transform.position;
                message_top.transform.position = new Vector2(pos.x, pos.y + 25);

                break;
            case 2:
                inventory.SetActive(false);
                OnMsg("적들이 깨지않게 조심히 움직여야 합니다.\nVision 을 켜 적의 시야를 확인하세요.", 999f);
                OnMark(vision, 999f);

                // Vision 킬 때 까지 STOP
                while (Mob.Mobs[0].gameObject.transform.Find("Vision").gameObject.active == false) yield return new WaitForSeconds(0.1f);
                OnMsg("비숍으로 이동하는편이\n좋을 것 같습니다.", 999f);
                OnMark(bishop, 999f);

                // 다음 아무 행동까지 STOP
                while (Player.action == Player.maxAction) yield return new WaitForSeconds(0.1f);
                OnMsg("모든 적을 섬멸하세요.", 3f);
                OffMark();

                break;
        }
    }

    /// <summary>
    /// Message_Top 에 text 를 seconds 만큼 출력시킵니다.
    /// </summary>
    public void OnMsg(string text, float seconds)
    {
        StartCoroutine(CorOnMsg(text, seconds));
    }
    public void OffMsg()
    {
        message_top.SetActive(false);
    }
    public IEnumerator CorOnMsg(string text, float seconds)
    {
        message_top.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        message_top.SetActive(true);
        this.text.text = text;
        yield return new WaitForSeconds(seconds);
        message_top.SetActive(false);
    }

    /// <summary>
    /// 지정한 오브젝트를 seconds 만큼 마킹합니다.
    /// </summary>
    public void OnMark(GameObject gameObject, float seconds)
    {
        StartCoroutine(CorOnMark(gameObject, seconds));
    }
    public void OffMark()
    {
        mark.SetActive(false);
    }
    public IEnumerator CorOnMark(GameObject gameObject, float seconds)
    {
        mark.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        mark.SetActive(true);
        mark.transform.position = gameObject.transform.position;
        yield return new WaitForSeconds(seconds);
        mark.SetActive(false);
    }
}
