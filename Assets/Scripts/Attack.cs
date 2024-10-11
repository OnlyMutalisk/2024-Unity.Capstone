using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    private Sprite select;

    private void Start()
    {
        select = Resources.Load<Sprite>("Images/Tile_Select_Attack");
    }

    /// <summary>
    /// 공격 가능한 타일을 표시합니다.
    /// </summary>
    public void OnOffTile()
    {
        GameManager.playerAction = GameManager.PlayerAction.Attack;

        if (Tile.isTileOn == false)
        {
            Tile.isTileOn = true;

            for (int n = 1; n <= GameManager.attackDistance_Char; n++)
            {
                Tile.AddTileImages(Grid.GetTile(Player.i - n, Player.j), A_Star.CheckMob(Player.i - n, Player.j));
                Tile.AddTileImages(Grid.GetTile(Player.i + n, Player.j), A_Star.CheckMob(Player.i + n, Player.j));
                Tile.AddTileImages(Grid.GetTile(Player.i, Player.j - n), A_Star.CheckMob(Player.i, Player.j - n));
                Tile.AddTileImages(Grid.GetTile(Player.i, Player.j + n), A_Star.CheckMob(Player.i, Player.j + n));
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
