using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    // 플레이어 초기 위치는 Transform 0, 0 기준으로 고정입니다.
    public static int i;
    public static int j;

    public Transform transform;
    public static Transform pos;
    public static bool isMove;
    public static int maxAction = GameManager.action_Char;
    public static int action = GameManager.action_Char;
    public static int Life = GameManager.Life_Char;

    private void Awake()
    {
        pos = transform;
        i = (Grid.i / 2) + 1;
        j = (Grid.j / 2) + 1;
    }

    /// <summary>
    /// 지정한 행렬 좌표로 이동합니다.
    /// </summary>
    public static IEnumerator CorMove(int i, int j)
    {
        if (isMove == false)
        {
            isMove = true;

            Vector3 target = pos.position;
            float diameter = Grid.cellSize + Grid.spacing;

            int xCount = j - Player.j;
            int yCount = i - Player.i;

            Player.j += xCount;
            target.x += xCount * diameter;
            Player.i += yCount;
            target.y += -yCount * diameter;

            for (int n = 0; n < Tile.tiles.Count; n++) { Tile.tiles[n].sprite = Tile.origins[n]; }
            Tile.tiles.Clear();
            Tile.origins.Clear();
            Tile.isTileOn = false;

            while (pos.position != target)
            {
                pos.position = Vector3.MoveTowards(pos.position, target, GameManager.speed_Char);
                yield return new WaitForSeconds(0.01f);
            }

            isMove = false;
        }
    }
}
