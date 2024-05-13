using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    public static bool isOn = false;
    public static Image tile;
    public static List<Image> tiles = new List<Image>();
    public static Sprite origin;
    public static List<Sprite> origins = new List<Sprite>();
    private Sprite select;

    private void Start()
    {
        select = Resources.Load<Sprite>("Images/Tile_Select");
    }

    public void Pawn()
    {
        tile = Grid.GetTile(Player.i - 1, Player.j).GetComponent<Image>();

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

    public void Rook()
    {
        tiles.Clear();

        for (int n = 1; n <= 3; n++)
        {
            tiles.Add(Grid.GetTile(Player.i - n, Player.j).GetComponent<Image>());
            tiles.Add(Grid.GetTile(Player.i + n, Player.j).GetComponent<Image>());
            tiles.Add(Grid.GetTile(Player.i, Player.j - n).GetComponent<Image>());
            tiles.Add(Grid.GetTile(Player.i, Player.j + n).GetComponent<Image>());
        }

        if (isOn == false)
        {
            isOn = true;
            foreach (Image tile in tiles)
            {
                origins.Add(tile.sprite);
                tile.sprite = select;
            }
        }
        else
        {
            isOn = false;

            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].sprite = origins[i];
            }

            foreach (Image tile in tiles)
            {
                tile.sprite = origin;
                origins.Clear();
            }
        }
    }
}
