using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public enum MoveStyle : int
{
    Default,
    Pawn,
    Knight,
    Bishop,
    Rook,
    None
}

public class Move : MonoBehaviour
{
    private Sprite select;
    private MoveStyle lastMove = MoveStyle.Default;

    private void Start()
    {
        select = Resources.Load<Sprite>("Images/Tile_Select");
    }

    public void Pawn()
    {
        GameManager.playerAction = GameManager.PlayerAction.Move;
        int range = Player.action / GameManager.cost_Pawn;

        if (Tile.isTileOn == false)
        {
            Tile.isTileOn = true;
            lastMove = MoveStyle.Pawn;

            for (int n = 1; n <= range; n++)
            {
                Tile.AddTileImages(Grid.GetTile(Player.i - n, Player.j));
                if (A_Star.CheckTile(Player.i - n, Player.j) == false) { break; }
            }

            foreach (Image tile in Tile.tiles)
            {
                Tile.origins.Add(tile.sprite);
                tile.sprite = select;
            }

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

            if (lastMove != MoveStyle.Default && lastMove != MoveStyle.Pawn) { Pawn(); }
        }
    }

    public void Knight()
    {
        GameManager.playerAction = GameManager.PlayerAction.Move;

        if (Tile.isTileOn == false)
        {
            Tile.isTileOn = true;
            lastMove = MoveStyle.Knight;

            if (A_Star.CheckTile(Player.i - 2, Player.j - 1) == true)
                Tile.AddTileImages(Grid.GetTile(Player.i - 2, Player.j - 1));
            if (A_Star.CheckTile(Player.i - 2, Player.j + 1) == true)
                Tile.AddTileImages(Grid.GetTile(Player.i - 2, Player.j + 1));
            if (A_Star.CheckTile(Player.i + 2, Player.j - 1) == true)
                Tile.AddTileImages(Grid.GetTile(Player.i + 2, Player.j - 1));
            if (A_Star.CheckTile(Player.i + 2, Player.j + 1) == true)
                Tile.AddTileImages(Grid.GetTile(Player.i + 2, Player.j + 1));
            if (A_Star.CheckTile(Player.i - 1, Player.j - 2) == true)
                Tile.AddTileImages(Grid.GetTile(Player.i - 1, Player.j - 2));
            if (A_Star.CheckTile(Player.i - 1, Player.j + 2) == true)
                Tile.AddTileImages(Grid.GetTile(Player.i - 1, Player.j + 2));
            if (A_Star.CheckTile(Player.i + 1, Player.j - 2) == true)
                Tile.AddTileImages(Grid.GetTile(Player.i + 1, Player.j - 2));
            if (A_Star.CheckTile(Player.i + 1, Player.j + 2) == true)
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

            if (lastMove != MoveStyle.Default && lastMove != MoveStyle.Knight) { Knight(); }
        }
    }

    public void Bishop()
    {
        GameManager.playerAction = GameManager.PlayerAction.Move;
        int range = Player.action / GameManager.cost_Bishop;

        if (Tile.isTileOn == false)
        {
            Tile.isTileOn = true;
            lastMove = MoveStyle.Bishop;

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

            if (lastMove != MoveStyle.Default && lastMove != MoveStyle.Bishop) { Bishop(); }
        }
    }

    public void Rook()
    {
        GameManager.playerAction = GameManager.PlayerAction.Move;
        int range = Player.action / GameManager.cost_Rook;

        if (Tile.isTileOn == false)
        {
            Tile.isTileOn = true;
            lastMove = MoveStyle.Rook;

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

            if (lastMove != MoveStyle.Default && lastMove != MoveStyle.Rook) { Rook(); }
        }
    }
}