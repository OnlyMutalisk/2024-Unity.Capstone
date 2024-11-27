using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public static List<Item> Items = new List<Item>();
    public Sprite sprite;
    public MonoBehaviour animation_wave;
    public int i;
    public int j;

    public IEnumerator CorMove(int i, int j)
    {
        Vector3 target = gameObject.transform.position;
        float diameter = Grid.cellSize + Grid.spacing;

        int xCount = j - this.j;
        int yCount = i - this.i;

        this.j += xCount;
        target.x += xCount * diameter;
        this.i += yCount;
        target.y += -yCount * diameter;

        while (gameObject.transform.position != target)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, GameManager.speed_Mob * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }

        animation_wave.enabled = true;
    }

    /// <summary>
    /// 플레이어 위치의 아이템을 탐지하고, 있다면 습득합니다.
    /// </summary>
    public static void CheckGetItem()
    {
        foreach (Item item in Items)
        {
            if (Player.i == item.i && Player.j == item.j)
            {
                Inventory.GetItem(item);
                Items.Remove(item);
                Destroy(item.gameObject);
                break;
            }
        }
    }

    /// <summary>
    /// 아이템을 사용합니다.
    /// </summary>
    public void Use()
    {
        switch (sprite.name)
        {
            case "Item_Shield":
                Shield();
                break;

            case "Item_Turn":
                AddTurn();
                break;
        }
    }

    /// <summary>
    /// 최대 실드량 보다 적다면, 실드를 회복합니다.
    /// </summary>
    private void Shield()
    {
        for (int i = 0; i < GameManager.recovery_shield; i++)
        {
            if (Player.shield < GameManager.shield_Char)
            {
                Player.shield++;
            }
            else
            {
                break;
            }
        }

        Mob.DrawLife();
    }

    /// <summary>
    /// Turn 을 증가시킵니다.
    /// </summary>
    private void AddTurn()
    {
        Turn.instance.turns += 3;
    }
}
