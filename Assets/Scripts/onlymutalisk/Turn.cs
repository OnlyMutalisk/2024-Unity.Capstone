using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Turn : MonoBehaviour
{
    public GameObject msg_top;
    public TextMeshProUGUI tmp;
    public GameObject[] controller;
    public static bool isMyTurn = true;
    private bool isStart = true;

    private void Update()
    {
        if (isStart == true) { isStart = false; StartCoroutine(UnitLoad()); }
        if (Player.action <= 0) { Player.action = Player.maxAction; StartCoroutine(EnemyTurn()); }
    }

    public IEnumerator UnitLoad()
    {
        ControlOnOff(false);
        tmp.text = GameManager.msg_loading;
        yield return new WaitForSeconds(GameManager.delay_loading);
        ControlOnOff(true);
    }

    public IEnumerator EnemyTurn()
    {
        ControlOnOff(false);
        tmp.text = GameManager.msg_turn;
        isMyTurn = false;

        foreach (var mob in Mob.Mobs)
        {
            mob.action = mob.maxAction;

            yield return StartCoroutine(mob.CorPlay());
        }

        isMyTurn = true;
        ControlOnOff(true);
    }

    public void ControlOnOff(bool OnOff)
    {
        msg_top.SetActive(!OnOff);

        foreach (var item in controller)
        {
            item.SetActive(OnOff);
        }
    }
}
