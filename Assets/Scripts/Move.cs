using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class Move : MonoBehaviour
{
    public static bool isOn = false;
    public static List<Image> tiles = new List<Image>();
    public static List<Sprite> origins = new List<Sprite>();
    private Sprite select;

    private void Start()
    {
        select = Resources.Load<Sprite>("Images/Tile_Select");
    }

    public void Pawn()
    {
        if (isOn == false)
        {
            isOn = true;

            tiles.Add(Grid.GetTile(Player.i - 1, Player.j).GetComponent<Image>());
            origins.Add(tiles[0].sprite);
            tiles[0].sprite = select;

            Tile.cost = GameManager.cost_Pawn;
        }
        else
        {
            isOn = false;

            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].sprite = origins[i];
            }

            tiles.Clear();
            origins.Clear();
        }
    }

    public void Knight()
    {
        if (isOn == false)
        {
            isOn = true;

            tiles.Add(Grid.GetTile(Player.i - 2, Player.j - 1).GetComponent<Image>());
            tiles.Add(Grid.GetTile(Player.i - 2, Player.j + 1).GetComponent<Image>());
            tiles.Add(Grid.GetTile(Player.i + 2, Player.j - 1).GetComponent<Image>());
            tiles.Add(Grid.GetTile(Player.i + 2, Player.j + 1).GetComponent<Image>());
            tiles.Add(Grid.GetTile(Player.i - 1, Player.j - 2).GetComponent<Image>());
            tiles.Add(Grid.GetTile(Player.i - 1, Player.j + 2).GetComponent<Image>());
            tiles.Add(Grid.GetTile(Player.i + 1, Player.j - 2).GetComponent<Image>());
            tiles.Add(Grid.GetTile(Player.i + 1, Player.j + 2).GetComponent<Image>());

            foreach (Image tile in tiles)
            {
                origins.Add(tile.sprite);
                tile.sprite = select;
            }

            Tile.cost = GameManager.cost_Knight;
        }
        else
        {
            isOn = false;

            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].sprite = origins[i];
            }

            tiles.Clear();
            origins.Clear();
        }
    }

    public void Bishop()
    {
        if (isOn == false)
        {
            isOn = true;

            for (int n = 1; n <= 3; n++)
            {
                tiles.Add(Grid.GetTile(Player.i - n, Player.j - n).GetComponent<Image>());
                tiles.Add(Grid.GetTile(Player.i - n, Player.j + n).GetComponent<Image>());
                tiles.Add(Grid.GetTile(Player.i + n, Player.j - n).GetComponent<Image>());
                tiles.Add(Grid.GetTile(Player.i + n, Player.j + n).GetComponent<Image>());
            }

            foreach (Image tile in tiles)
            {
                origins.Add(tile.sprite);
                tile.sprite = select;
            }

            Tile.cost = GameManager.cost_Bishop;
        }
        else
        {
            isOn = false;

            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].sprite = origins[i];
            }

            tiles.Clear();
            origins.Clear();
        }
    }

    public void Rook()
    {
        if (isOn == false)
        {
            isOn = true;

            for (int n = 1; n <= 3; n++)
            {
                tiles.Add(Grid.GetTile(Player.i - n, Player.j).GetComponent<Image>());
                tiles.Add(Grid.GetTile(Player.i + n, Player.j).GetComponent<Image>());
                tiles.Add(Grid.GetTile(Player.i, Player.j - n).GetComponent<Image>());
                tiles.Add(Grid.GetTile(Player.i, Player.j + n).GetComponent<Image>());
            }

            foreach (Image tile in tiles)
            {
                origins.Add(tile.sprite);
                tile.sprite = select;
            }

            Tile.cost = GameManager.cost_Rook;
        }
        else
        {
            isOn = false;

            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].sprite = origins[i];
            }

            tiles.Clear();
            origins.Clear();
        }
    }
}