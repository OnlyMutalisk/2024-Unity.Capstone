using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class Item : MonoBehaviour
{
    public static List<Item> Items = new List<Item>();
    public Sprite sprite; // 인스턴스화 시 할당됨
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
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, GameManager.speed_Mob);
            yield return new WaitForSeconds(0.01f);
        }
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

    }
}
