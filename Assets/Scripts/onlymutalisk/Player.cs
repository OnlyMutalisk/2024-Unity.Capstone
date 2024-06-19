using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
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
    public static int maxAction;
    public static int action;
    public static int Life;

    private void Awake()
    {
        maxAction = GameManager.action_Char;
        action = GameManager.action_Char;
        Life = GameManager.Life_Char;

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
            A_Star.SwitchWall(Player.i, Player.j);
            A_Star.SwitchWall(i, j);

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

            // 플레이어가 몬스터의 시야범위 이내로 이동하면 몬스터를 깨움
            foreach (var mob in Mob.Mobs)
            {
                if (Mathf.Max(Mathf.Abs(mob.i - Player.i), Mathf.Abs(mob.j - Player.j)) <= mob.visionRange)
                {
                    mob.isSleep = false;
                }
            }

            isMove = false;
        }
    }
}
