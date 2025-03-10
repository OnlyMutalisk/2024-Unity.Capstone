using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Turn : MonoBehaviour
{
    public static Turn instance;
    public TextMeshProUGUI turn;
    public GameObject lose;
    public GameObject win;
    public GameObject life;
    public List<GameObject> controller;
    public static bool isMyTurn = true;
    public int turns = 10;
    private bool isStart = true;

    private void Awake()
    {
        isMyTurn = true;
        turns = GameManager.turns[Map.index];
        instance = this;
        StartCoroutine(TurnScanner());
    }

    private IEnumerator TurnScanner()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (isStart == true) { isStart = false; StartCoroutine(UnitLoad()); }
            if (Player.action <= 0 && Tile.isNowSkill == false) { Player.action = Player.maxAction; StartCoroutine(EnemyTurn()); }
            if (Player.life <= 0 || turns == 0) { StartCoroutine(Lose()); break; }

            // 상자를 제외한 몬스터의 수가 0 이면 승리
            int chestsCount = 0;
            foreach (var item in Mob.Mobs) if (item is Chest) chestsCount++;
            if (Mob.Mobs.Count - chestsCount == 0 && Mob.mobCounting == true) { StartCoroutine(Win()); break; }
            turn.text = turns.ToString();
        }
    }

    private IEnumerator Win()
    {
        Stages.isOn[Map.index + 1] = true;
        yield return new WaitForSeconds(0.5f);
        win.SetActive(true);
    }

    private IEnumerator Lose()
    {
        Player.anim.SetBool("isDeath", true);
        life.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        lose.SetActive(true);
    }

    public IEnumerator UnitLoad()
    {
        ControlOnOff(false);
        Scenario.instance.OnMsg(GameManager.msg_loading, GameManager.delay_loading);
        yield return new WaitForSeconds(GameManager.delay_loading);
        ControlOnOff(true);
    }

    public IEnumerator EnemyTurn()
    {
        ControlOnOff(false);
        if (Tile.isTileOn == true) OffTile();
        void OffTile()
        {
            Tile.isTileOn = false;

            for (int i = 0; i < Tile.tiles.Count; i++)
            {
                Tile.tiles[i].sprite = Tile.origins[i];
            }

            Tile.tiles.Clear();
            Tile.origins.Clear();
        }
        Vision.instance.VisionOnOff(false);

        Scenario.instance.OnMsg(GameManager.msg_turn, 999f);
        isMyTurn = false;
        turns--;

        foreach (var mob in Mob.Mobs)
        {
            mob.action = mob.maxAction;

            yield return StartCoroutine(mob.CorPlay());
        }

        isMyTurn = true;
        ControlOnOff(true);
        Scenario.instance.OffMsg();
    }

    public void ControlOnOff(bool OnOff)
    {
        foreach (var item in controller)
        {
            item.SetActive(OnOff);
        }
    }
}
