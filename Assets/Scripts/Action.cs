using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    public GameObject message_mid;
    public GameObject panel_action;

    public void OpenMsg_Action()
    {
        if (Turn.isMyTurn == true)
        {
            message_mid.SetActive(true);
            panel_action.SetActive(true);
        }
    }

    public void TurnOver()
    {
        Player.action = 0;
    }
}
