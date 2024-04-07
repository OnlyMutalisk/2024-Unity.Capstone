using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public void CheckAction()
    {
        if (GameManager.playerAction == GameManager.PlayerAction.Move)
        {
            Player.Move();
        }
        if (GameManager.playerAction == GameManager.PlayerAction.Attack)
        {
            Player.Attack();
        }
        if (GameManager.playerAction == GameManager.PlayerAction.Skill)
        {
            Player.Skill();
        }
    }

    public void Test()
    {
        Debug.Log("ㅇㅇ");
    }
}
