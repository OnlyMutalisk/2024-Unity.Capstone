using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class Move : MonoBehaviour
{
    private Sprite select;

    private void Start()
    {
        select = Resources.Load<Sprite>("Images/Tile_Select");
    }

    public void Pawn()
    {
        GameManager.playerAction = GameManager.PlayerAction.Move;

        if (Tile.isTileOn == false)
        {
            Tile.isTileOn = true;

            Tile.AddTileImages(Grid.GetTile(Player.i - 1, Player.j));
            Tile.origins.Add(Tile.tiles[0].sprite);
            Tile.tiles[0].sprite = select;

            Tile.cost = GameManager.cost_Pawn;
        }
        else
        {
            Tile.isTileOn = false;

            for (int i = 0; i < Tile.tiles.Count; i++)
            {
                Tile.tiles[i].sprite = Tile.origins[i];
            }

            Tile.tiles.Clear();
            Tile.origins.Clear();
        }
    }

    public void Knight()
    {
        GameManager.playerAction = GameManager.PlayerAction.Move;

        if (Tile.isTileOn == false)
        {
            Tile.isTileOn = true;

            Tile.AddTileImages(Grid.GetTile(Player.i - 2, Player.j - 1));
            Tile.AddTileImages(Grid.GetTile(Player.i - 2, Player.j + 1));
            Tile.AddTileImages(Grid.GetTile(Player.i + 2, Player.j - 1));
            Tile.AddTileImages(Grid.GetTile(Player.i + 2, Player.j + 1));
            Tile.AddTileImages(Grid.GetTile(Player.i - 1, Player.j - 2));
            Tile.AddTileImages(Grid.GetTile(Player.i - 1, Player.j + 2));
            Tile.AddTileImages(Grid.GetTile(Player.i + 1, Player.j - 2));
            Tile.AddTileImages(Grid.GetTile(Player.i + 1, Player.j + 2));

            foreach (Image tile in Tile.tiles)
            {
                Tile.origins.Add(tile.sprite);
                tile.sprite = select;
            }

            Tile.cost = GameManager.cost_Knight;
        }
        else
        {
            Tile.isTileOn = false;

            for (int i = 0; i < Tile.tiles.Count; i++)
            {
                Tile.tiles[i].sprite = Tile.origins[i];
            }

            Tile.tiles.Clear();
            Tile.origins.Clear();
        }
    }

    public void Bishop()
    {
        GameManager.playerAction = GameManager.PlayerAction.Move;
        int range = Player.action / GameManager.cost_Bishop;
        
        if (Tile.isTileOn == false)
        {
            Tile.isTileOn = true;

            for (int n = 1; n <= range; n++)
            {
                Tile.AddTileImages(Grid.GetTile(Player.i - n, Player.j - n));
                if (A_Star.CheckTile(Player.i - n, Player.j - n) == false) { break; }
            }

            for (int n = 1; n <= range; n++)
            {
                Tile.AddTileImages(Grid.GetTile(Player.i - n, Player.j + n));
                if (A_Star.CheckTile(Player.i - n, Player.j + n) == false) { break; }
            }

            for (int n = 1; n <= range; n++)
            {
                Tile.AddTileImages(Grid.GetTile(Player.i + n, Player.j - n));
                if (A_Star.CheckTile(Player.i + n, Player.j - n) == false) { break; }
            }

            for (int n = 1; n <= range; n++)
            {
                Tile.AddTileImages(Grid.GetTile(Player.i + n, Player.j + n));
                if (A_Star.CheckTile(Player.i + n, Player.j + n) == false) { break; }
            }

            foreach (Image tile in Tile.tiles)
            {
                Tile.origins.Add(tile.sprite);
                tile.sprite = select;
            }

            Tile.cost = GameManager.cost_Bishop;
        }
        else
        {
            Tile.isTileOn = false;

            for (int i = 0; i < Tile.tiles.Count; i++)
            {
                Tile.tiles[i].sprite = Tile.origins[i];
            }

            Tile.tiles.Clear();
            Tile.origins.Clear();
        }
    }

    public void Rook()
    {
        GameManager.playerAction = GameManager.PlayerAction.Move;
        int range = Player.action / GameManager.cost_Rook;

        if (Tile.isTileOn == false)
        {
            Tile.isTileOn = true;

            for (int n = 1; n <= range; n++)
            {
                Tile.AddTileImages(Grid.GetTile(Player.i - n, Player.j));
                if (A_Star.CheckTile(Player.i - n, Player.j) == false) { break; }
            }

            for (int n = 1; n <= range; n++)
            {
                Tile.AddTileImages(Grid.GetTile(Player.i + n, Player.j));
                if (A_Star.CheckTile(Player.i + n, Player.j) == false) { break; }
            }

            for (int n = 1; n <= range; n++)
            {
                Tile.AddTileImages(Grid.GetTile(Player.i, Player.j - n));
                if (A_Star.CheckTile(Player.i, Player.j - n) == false) { break; }
            }

            for (int n = 1; n <= range; n++)
            {
                Tile.AddTileImages(Grid.GetTile(Player.i, Player.j + n));
                if (A_Star.CheckTile(Player.i, Player.j + n) == false) { break; }
            }

            foreach (Image tile in Tile.tiles)
            {
                Tile.origins.Add(tile.sprite);
                tile.sprite = select;
            }

            Tile.cost = GameManager.cost_Rook;
        }
        else
        {
            Tile.isTileOn = false;

            for (int i = 0; i < Tile.tiles.Count; i++)
            {
                Tile.tiles[i].sprite = Tile.origins[i];
            }

            Tile.tiles.Clear();
            Tile.origins.Clear();
        }
    }
}