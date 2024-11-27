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
    public static Scenario instance;
    private static Coroutine lastCor;

    public Turn turn_script;
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

    // 싱글턴
    private void Awake() { instance = this; }

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
                action.SetActive(false); turn_script.controller.Remove(action);
                skill.SetActive(false); turn_script.controller.Remove(skill);
                Player.action = 999;
                OnMsg("Pawn 을 클릭해 전진하세요!", 999f);
                OnMark(pawn, 999f);

                // Pawn 클릭할 때 까지 STOP
                while (Grid.GetTile(Player.i - 1, Player.j).GetComponent<Image>().sprite.name != "Tile_Select") yield return new WaitForSeconds(0.5f);
                OnMsg("하늘색 땅을 클릭해 전진하세요!", 999f);
                OnMark(Grid.GetTile(Player.i - 1, Player.j), 999);

                // 전진할 때 까지 STOP
                while (Player.action == 999) yield return new WaitForSeconds(0.5f);
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
                turn.SetActive(false);
                inventory.SetActive(false);
                vision.SetActive(false);
                knight.SetActive(false);
                bishop.SetActive(false);
                rook.SetActive(false);
                skill.SetActive(false); turn_script.controller.Remove(skill);
                Player.action = 2;
                OnMsg("폰을 눌러 이동하세요 !", 999f);
                OnMark(pawn, 999f);

                // 폰을 누를 때 까지 Stop
                while (Grid.GetTile(Player.i - 1, Player.j).GetComponent<Image>().sprite.name != "Tile_Select") yield return new WaitForSeconds(0.1f);
                OnMsg("남은 행동력이 2 이므로\n2칸 이내로 이동할 수 있습니다.", 999f);
                OnMark(action, 999f);
                // 액션 버튼 뽑아버리기

                // 다시 내 턴 될 때 까지 Stop
                while (Player.action != Player.maxAction) yield return new WaitForSeconds(0.1f);
                OnMsg("아직 깨어난 몬스터가 없으므로\n다시 당신의 턴 입니다.", 5f);
                OffMark();

                // 적이 깨어날 때 까지 Stop
                while (Mob.Mobs[0].isSleep == true) yield return new WaitForSeconds(0.1f);
                OnMsg("적이 깨어나 턴을 부여받습니다.", 2f);

                // 다시 나의 턴이 될 때 까지 Stop
                while (Player.action != Player.maxAction) yield return new WaitForSeconds(0.1f);
                pawn.SetActive(false);
                OnMsg("적을 꾸욱 눌러보세요.", 999f);

                // 적을 꾸욱 누를 때 까지 Stop
                while (Mob.Mobs[0].transform.Find("Enemy_Vision").gameObject.active == false) yield return new WaitForSeconds(0.1f);
                OnMsg("영역 이내로 들어가면 적이 깨어납니다.", 2f);

                break;
            case 2:
                inventory.SetActive(false);
                vision.SetActive(false);
                bishop.SetActive(false);
                rook.SetActive(false);
                skill.SetActive(false); turn_script.controller.Remove(skill);
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

                // 턴 종료할 때 까지 STOP
                while (Player.action != Player.maxAction) yield return new WaitForSeconds(0.5f);
                OnMsg("남은 턴 수 내에 클리어하지 못하면 패배합니다. 서두르세요 !", 5f);
                Vector2 pos = message_top.transform.position;
                message_top.transform.position = new Vector2(pos.x, pos.y - 25);
                OnMark(turn, 5f);

                // 적 조우 시 STOP
                while (A_Star.GetDistance(Grid.GetTile(Player.i, Player.j).GetComponent<Tile>(), Grid.GetTile(Mob.Mobs[0].i, Mob.Mobs[0].j).GetComponent<Tile>(), RangeType.Manhattan) > 4) yield return new WaitForSeconds(0.5f);
                OnMsg("조심하세요 ! 적이 원거리 공격을 사용합니다.", 10f);
                OffMark();
                pos = message_top.transform.position;
                message_top.transform.position = new Vector2(pos.x, pos.y + 25);

                break;
            case 3:
                inventory.SetActive(false);
                pawn.SetActive(false);
                knight.SetActive(false);
                bishop.SetActive(false);
                rook.SetActive(false);
                skill.SetActive(false); turn_script.controller.Remove(skill);
                OnMsg("적들이 깨지않게 조심히 움직여야 합니다.\nVision 을 켜 적의 시야를 확인하세요.", 999f);
                OnMark(vision, 999f);

                // Vision 킬 때 까지 STOP
                while (Mob.Mobs[0].gameObject.transform.Find("Enemy_Vision").gameObject.active == false) yield return new WaitForSeconds(0.1f);
                OnMsg("비숍으로 움직이는게 좋을 것 같습니다.", 999f);
                bishop.SetActive(true);
                OnMark(bishop, 999f);

                // 비숍 누를 때 까지 STOP
                while (Grid.GetTile(Player.i - 1, Player.j - 1).GetComponent<Image>().sprite.name != "Tile_Select") yield return new WaitForSeconds(0.1f);
                OnMsg("적의 시야를 넘어 걸리지 않고 이동하세요.", 999f);
                OnMark(Grid.GetTile(Player.i - 3, Player.j - 3), 999f);

                // 다음 아무 행동까지 STOP
                while (Player.action == Player.maxAction) yield return new WaitForSeconds(0.1f);
                OnMsg("모든 적을 섬멸하세요.", 3f);
                pawn.SetActive(true);
                knight.SetActive(true);
                rook.SetActive(true);
                OffMark();

                break;
            case 4:
                skill.SetActive(false); turn_script.controller.Remove(skill);
                OnMsg("실드를 획득하세요.", 999f);
                int shortest = 999;
                GameObject target = null;
                foreach (var item in Item.Items)
                {
                    int distance = A_Star.GetDistance(Grid.GetTile(item.i, item.j).GetComponent<Tile>(), Grid.GetTile(Player.i, Player.j).GetComponent<Tile>(), RangeType.Manhattan);
                    if (shortest > distance)
                    {
                        shortest = distance;
                        target = Grid.GetTile(item.i, item.j);
                    }
                }
                yield return new WaitForSeconds(0.5f);
                OnMark(target, 999f);

                // 실드를 획득할 때 까지 STOP
                while (Inventory.InvItems.Count < 1) yield return new WaitForSeconds(0.1f);
                OnMsg("인벤토리를 열어보세요.", 999f);
                OnMark(inventory, 999f);

                // 인벤토리 열 때 까지 STOP
                while (inventory.transform.GetChild(0).gameObject.active == false) yield return new WaitForSeconds(0.1f);
                OnMsg("더블 클릭하여 실드를 사용하세요.", 999f);
                OffMark();

                // 인벤토리를 끄거나, 실드 사용할 때 까지 STOP
                while (Player.shield == 0) yield return new WaitForSeconds(0.1f);
                OnMsg("모든 적을 물리치세요 !", 3f);

                break;
            case 5:
                OnMsg("다른 방법이 없습니다.\n스킬을 사용해 적을 제거하세요 !", 999f);
                OnMark(skill, 999f);

                // Skill 버튼을 누를 때 까지 Stop
                while (Grid.GetTile(Player.i - 2, Player.j + 1).GetComponent<Image>().sprite.name != "Tile_Select_Skill") yield return new WaitForSeconds(0.1f);
                OnMark(Grid.GetTile(Player.i - 2, Player.j + 1), 999f);

                // Skill 을 사용할 때 까지 Stop
                while (Player.action == Player.maxAction) yield return new WaitForSeconds(0.1f);
                OffMsg();
                OffMark();

                break;
            case 6:
                OnMsg("한 가지 팁을 주자면,\n상대 턴에도 아이템을 사용할 수 있습니다.", 6f);
                break;
            case 7:
                OnMsg("이번 맵은 Turn 이 부족합니다.\nTurn 아이템을 파밍하세요. ", 6f);
                break;
        }
    }

    /// <summary>
    /// Message_Top 에 text 를 seconds 만큼 출력시킵니다.
    /// </summary>
    public void OnMsg(string text, float seconds)
    {
        Audio.instance.PlaySfx(Audio.Sfx.Message);
        OffMsg();
        if (lastCor != null) StopCoroutine(lastCor);
        lastCor = StartCoroutine(CorOnMsg(text, seconds));
    }
    public void OffMsg()
    {
        message_top.SetActive(false);
    }
    private IEnumerator CorOnMsg(string text, float seconds)
    {
        message_top.SetActive(true);
        this.text.text = text;
        yield return new WaitForSeconds(seconds);
        message_top.SetActive(false);
    }

    /// <summary>
    /// 지정한 오브젝트를 seconds 만큼 마킹합니다.
    /// </summary>
    public static List<Coroutine> CorsMark = new List<Coroutine>();
    public void OnMark(GameObject gameObject, float seconds)
    {
        foreach (var item in CorsMark) StopCoroutine(item);
        CorsMark.Clear();
        CorsMark.Add(StartCoroutine(CorOnMark(gameObject, seconds)));
        CorsMark.Add(StartCoroutine(CorTraceMark(gameObject, seconds)));
    }
    public void OffMark()
    {
        mark.SetActive(false);
    }
    private IEnumerator CorOnMark(GameObject gameObject, float seconds)
    {
        mark.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        mark.SetActive(true);
        yield return new WaitForSeconds(seconds);
        mark.SetActive(false);
    }
    private IEnumerator CorTraceMark(GameObject gameObject, float seconds)
    {
        float sec = 0;

        while (sec < seconds)
        {
            sec += 0.01f;
            if (gameObject != null) mark.transform.position = gameObject.transform.position;
            yield return new WaitForSeconds(0.01f);
        }
    }
}