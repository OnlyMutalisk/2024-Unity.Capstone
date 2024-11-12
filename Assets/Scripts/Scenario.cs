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
                skill.SetActive(false);
                knight.SetActive(false);
                bishop.SetActive(false);
                rook.SetActive(false);
                OnMsg("Pawn 을 클릭해 전진하세요!", 999f);
                OnMark(pawn, 999f);

                // Pawn 클릭할 때 까지 시나리오 STOP
                while (Grid.GetTile(Player.i - 1, Player.j).GetComponent<Image>().sprite.name != "Tile_Select") yield return new WaitForSeconds(0.5f);
                OnMsg("하늘색 땅을 클릭해 전진하세요!", 999f);
                OnMark(Grid.GetTile(Player.i - 1, Player.j), 999);

                // 전진할 때 까지 시나리오 STOP
                while (Player.action == Player.maxAction) yield return new WaitForSeconds(0.5f);
                OnMsg("앞으로 계속 나아가 적을 마주하세요!", 999f);
                OffMark();

                // 몬스터와 조우할 때 까지 시나리오 STOP
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

                // 공격 버튼을 누를 때 까지 시나리오 STOP
                while (Grid.GetTile(Player.i - 1, Player.j).GetComponent<Image>().sprite.name != "Tile_Select_Attack") yield return new WaitForSeconds(0.5f);
                OnMsg("적을 무찌르세요 !", 999f);
                OnMark(enemy, 999f);
                int maxMobCount = Mob.Mobs.Count;

                // 한 마리를 처치할 때 까지 시나리오 STOP
                while (maxMobCount == Mob.Mobs.Count) yield return new WaitForSeconds(0.5f);
                OnMsg("모든 적을 무찌르고 스테이지를 클리어하세요.", 999f);
                OffMark();
                Player.life = GameManager.Life_Char;
                Mob.DrawLife();

                break;
            case 1:
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
