using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    public void LoadGame(int index)
    {
        SceneManager.LoadScene("Game");
        Map.index = index;
    }

    public void LoadMain()
    {
        if (Tile.isTileOn == true) OffTile();

        SceneManager.LoadScene("Main");

        void OffTile()
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
