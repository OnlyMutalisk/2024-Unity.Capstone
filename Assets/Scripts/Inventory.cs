using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class Inventory : MonoBehaviour
{
    public GameObject inv;
    public Image[] slots;
    public static Image[] s_slots;
    public static List<Item> InvItems = new List<Item>();
    private static int maxItem;

    public void Start()
    {
        s_slots = slots;
    }

    public static void GetItem(Item item)
    {
        // 최대 아이템 수를 넘어가면, 마지막 아이템 삭제 후 획득
        if (InvItems.Count > maxItem)
        {
            InvItems.RemoveAt(InvItems.Count - 1);
            InvItems.Add(item);
        }
        else
        {
            InvItems.Add(item);
        }

        UpdateSlot();
    }

    public void UseItem(int index)
    {
        if (index < InvItems.Count)
        {
            Item item = InvItems[index];
            item.Use();
            InvItems.Remove(item);

            UpdateSlot();
        }
    }

    /// <summary>
    /// 인벤토리 슬롯의 스프라이트를 갱신합니다.
    /// </summary>
    public static void UpdateSlot()
    {
        for (int i = 0; i < s_slots.Length; i++)
        {
            if (i < InvItems.Count)
            {
                s_slots[i].sprite = InvItems[i].sprite;
            }
            else
            {
                s_slots[i].sprite = null;
            }
        }
    }

    public void InvOnOff()
    {
        inv.SetActive(!inv.active);
    }
}
