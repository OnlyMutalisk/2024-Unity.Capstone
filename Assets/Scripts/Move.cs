using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    public static bool isOn = false;
    private Image tile;
    public static Sprite origin;
    private Sprite select;

    public void Pawn()
    {
        tile = Grid.GetTile(Player.i - 1, Player.j).GetComponent<Image>();
        select = Resources.Load<Sprite>("Images/Tile_Select");

        if (isOn == false)
        {
            isOn = true;
            origin = tile.sprite;
            tile.sprite = select;
        }
        else
        {
            isOn = false;
            tile.sprite = origin;
        }
    }

}
