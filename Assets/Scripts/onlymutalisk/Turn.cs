using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{
    public GameObject message;
    public GameObject[] controller;

    private void Update()
    {
        if (Player.action <= 0) { Player.action = Player.maxAction; StartCoroutine(EnemyTurn()); }
    }

    public IEnumerator EnemyTurn()
    {
        ControlOnOff(false);

        foreach (var mob in Mob.Mobs)
        {
            mob.action = mob.maxAction;

            yield return StartCoroutine(mob.CorPlay());
        }

        ControlOnOff(true);
    }

    public void ControlOnOff(bool OnOff)
    {
        message.SetActive(!OnOff);

        foreach (var item in controller)
        {
            item.SetActive(OnOff);
        }
    }
}
