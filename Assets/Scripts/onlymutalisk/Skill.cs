using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    private Sprite select;

    private void Start()
    {
        select = Resources.Load<Sprite>("Images/Tile_Select_Skill");
    }

    /// <summary>
    /// 공격 가능한 타일을 표시합니다.
    /// </summary>
    public void OnOffTile()
    {
        GameManager.playerAction = GameManager.PlayerAction.Skill;

        if (Tile.isTileOn == false)
        {
            Tile.isTileOn = true;
            List<(int, int)> coordinates = new List<(int, int)>();

            for (int n = 1; n <= GameManager.skillDistance_Char; n++)
            {
                for (int x = -n; x <= n; x++)
                {
                    for (int y = -n; y <= n; y++)
                    {
                        if (Math.Max(Math.Abs(x), Math.Abs(y)) == n)
                        {
                            coordinates.Add((x, y));
                        }
                    }
                }
            }

            foreach (var coord in coordinates)
            {
                Tile.AddTileImages(Grid.GetTile(Player.i + coord.Item1, Player.j + coord.Item2), A_Star.CheckMob(Player.i + coord.Item1, Player.j + coord.Item2));
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
