using System;
using System.Collections.Generic;
using UnityEngine;

// 몬스터의 이동 타입입니다.
public enum RangeType
{
    Manhattan,
    Chebyshev,
    Pawn,
    Knight,
    Bishop,
}

public class A_Star : MonoBehaviour
{
    // Tile 객체가 각각의 타일 역할을 수행합니다.
    public static List<Tile> tiles_close = new List<Tile>();
    public static List<Tile> tiles_open = new List<Tile>();

    /// <summary>
    /// <br>유저를 공격할 수 있는 가장 가까운 경로의 첫 번째 타일(타일)을 반환합니다.</br>
    /// </summary>
    public static List<Tile> PathFind(Tile startTile, RangeType rangeType, int distance = 1)
    {
        // 유저를 공격할 수 있는 공격범위 내 타일을 타겟으로 지정
        List<Tile> neighbortiles = GetNeighbortiles(Grid.GetTile(Player.i, Player.j).GetComponent<Tile>(), distance, GameManager.rangeType_enemy);

        // 각 타겟으로의 최소 경로들을 저장
        List<List<Tile>> pathList = new List<List<Tile>>();

        // 각 타겟으로의 최소경로 구하기
        foreach (Tile destination in neighbortiles)
        {
            // 초기화
            List<Tile> path = new List<Tile>();
            tiles_close.Clear();
            tiles_open.Clear();
            tiles_open.Add(startTile);

            // 가능한 모든 경로 탐색
            while (tiles_open.Count > 0)
            {
                Tile currentTile = tiles_open[0];

                // currentTile 를 F가 가장 작은 타일로 변경
                for (int i = 1; i < tiles_open.Count; i++)
                {
                    if (tiles_open[i].F <= currentTile.F)
                    {
                        currentTile = tiles_open[i];
                    }
                }

                // 오픈 타일에서 클로즈 타일로 이동
                tiles_open.Remove(currentTile);
                tiles_close.Add(currentTile);

                // 타겟에 도달 시 path 에 경로 삽입
                if (Tile.Equals(currentTile, destination) == true)
                {
                    // 시작 타일에 도달할 때 까지 거슬러 올라가며, path 에 추가
                    Tile cTile = currentTile;
                    while (!(cTile.i == startTile.i && cTile.j == startTile.j))
                    {
                        path.Add(cTile);
                        cTile = cTile.parentsTile;
                    }

                    path.Reverse();

                    // 각 타일의 부모 수를 기록합니다.
                    for (int i = 0; i < path.Count; i++)
                    {
                        path[i].parentsCount = i + 1;
                    }

                    if (path.Count != 0) { pathList.Add(path); }
                    break;
                }

                // 오픈 타일에 다음 이웃 타일 추가
                AddOpenTiles(currentTile, startTile, destination, 1, rangeType);
            }
        }

        // 타겟을 향한 모든 경로 중, 최소 경로를 선택합니다. parentsCount 값을 기준으로 결정합니다.
        if (pathList.Count != 0)
        {
            List<Tile> smallestPath = pathList[0];

            for (int i = 1; i < pathList.Count; i++)
            {
                if (pathList[i][pathList[i].Count - 1].parentsCount < smallestPath[smallestPath.Count - 1].parentsCount)
                {
                    smallestPath = pathList[i];
                }
            }

            return smallestPath;
        }
        else
        {
            return PathFind(startTile, rangeType, distance + 1);
        }
    }

    /// <summary>
    /// <br>일정 거리 이내의 이웃 타일을 반환합니다.</br>
    /// <br>체스 RangeType은 distance 의 영향 없이 이웃 타일이 결정됩니다.</br>
    /// </summary>
    public static List<Tile> GetNeighbortiles(Tile tile, int distance, RangeType type = RangeType.Manhattan)
    {
        List<Tile> neighbors = new List<Tile>();

        int i = tile.i;
        int j = tile.j;

        if (type == RangeType.Manhattan)
        {
            // 행렬 기준 거리 내의 모든 타일에 대해서
            for (int di = -distance; di <= distance; di++)
            {
                for (int dj = -distance; dj <= distance; dj++)
                {
                    if (di == 0 && dj == 0) continue; // 자기 자신은 스킵

                    int ni = i + di;
                    int nj = j + dj;

                    if (CheckTile(ni, nj) == true)
                    {
                        if (Math.Abs(di) + Math.Abs(dj) <= distance)
                        {
                            neighbors.Add(Grid.GetTile(ni, nj).GetComponent<Tile>());
                        }
                    }
                }
            }
        }
        else if (type == RangeType.Chebyshev)
        {
            // 행렬 기준 거리 내의 모든 타일에 대해서
            for (int di = -distance; di <= distance; di++)
            {
                for (int dj = -distance; dj <= distance; dj++)
                {
                    if (di == 0 && dj == 0) continue; // 자기 자신은 스킵

                    int ni = i + di;
                    int nj = j + dj;

                    if (CheckTile(ni, nj) == true)
                    {
                        if (Math.Max(Math.Abs(di), Math.Abs(dj)) <= distance)
                        {
                            neighbors.Add(Grid.GetTile(ni, nj).GetComponent<Tile>());
                        }
                    }
                }
            }
        }
        else if (type == RangeType.Pawn)
        {
            if (CheckTile(i, j + 1) == true) { neighbors.Add(Grid.GetTile(i, j + 1).GetComponent<Tile>()); }
            if (CheckTile(i, j - 1) == true) { neighbors.Add(Grid.GetTile(i, j - 1).GetComponent<Tile>()); }
            if (CheckTile(i + 1, j) == true) { neighbors.Add(Grid.GetTile(i + 1, j).GetComponent<Tile>()); }
            if (CheckTile(i - 1, j) == true) { neighbors.Add(Grid.GetTile(i - 1, j).GetComponent<Tile>()); }
        }
        else if (type == RangeType.Knight)
        {
            if (CheckTile(i - 2, j + 1) == true) { neighbors.Add(Grid.GetTile(i - 2, j + 1).GetComponent<Tile>()); }
            if (CheckTile(i - 2, j - 1) == true) { neighbors.Add(Grid.GetTile(i - 2, j - 1).GetComponent<Tile>()); }
            if (CheckTile(i - 1, j + 2) == true) { neighbors.Add(Grid.GetTile(i - 1, j + 2).GetComponent<Tile>()); }
            if (CheckTile(i - 1, j - 2) == true) { neighbors.Add(Grid.GetTile(i - 1, j - 2).GetComponent<Tile>()); }
            if (CheckTile(i + 1, j + 2) == true) { neighbors.Add(Grid.GetTile(i + 1, j + 2).GetComponent<Tile>()); }
            if (CheckTile(i + 1, j - 2) == true) { neighbors.Add(Grid.GetTile(i + 1, j - 2).GetComponent<Tile>()); }
            if (CheckTile(i + 2, j + 1) == true) { neighbors.Add(Grid.GetTile(i + 2, j + 1).GetComponent<Tile>()); }
            if (CheckTile(i + 2, j - 1) == true) { neighbors.Add(Grid.GetTile(i + 2, j - 1).GetComponent<Tile>()); }
        }
        else if (type == RangeType.Bishop)
        {
            if (CheckTile(i - 1, j + 1) == true) { neighbors.Add(Grid.GetTile(i - 1, j + 1).GetComponent<Tile>()); }
            if (CheckTile(i - 1, j - 1) == true) { neighbors.Add(Grid.GetTile(i - 1, j - 1).GetComponent<Tile>()); }
            if (CheckTile(i + 1, j + 1) == true) { neighbors.Add(Grid.GetTile(i + 1, j + 1).GetComponent<Tile>()); }
            if (CheckTile(i + 1, j - 1) == true) { neighbors.Add(Grid.GetTile(i + 1, j - 1).GetComponent<Tile>()); }
        }

        // 경로가 없다면, 가까운 타일을 경로로 잡기위해 재귀호출합니다.
        if (neighbors.Count == 0) { neighbors = GetNeighbortiles(tile, distance + 1, RangeType.Chebyshev); }

        return neighbors;
    }

    /// <summary>
    /// <br> 근처 타일을 오픈 타일 리스트에 추가합니다.</br>
    /// </summary>
    private static void AddOpenTiles(Tile tile, Tile startTile, Tile dstTile, int distance, RangeType type = RangeType.Manhattan)
    {
        List<Tile> tiles = GetNeighbortiles(tile, distance, type);

        foreach (Tile item in tiles)
        {
            // 타일이 유효하고 열린 타일 & 닫힌 타일 리스트에 없다면 openNodes 에 추가
            if (!tiles_open.Contains(item) && !tiles_close.Contains(item))
            {
                item.G = GetDistance(startTile, item, RangeType.Manhattan);
                item.H = GetDistance(item, dstTile, RangeType.Manhattan);
                item.F = item.G + item.H;
                item.parentsTile = tile;
                tiles_open.Add(item);
            }
        }
    }

    /// <summary>
    /// <br>행렬 좌표 [i][j] 의 타일이 타일 맵 안에 있고, 벽 판정이 아니라면 true 를 반환합니다.</br>
    /// </summary>
    public static bool CheckTile(int i, int j)
    {
        if ((0 < i && i <= Grid.i) && (0 < j && j <= Grid.j) == true)
            if (Grid.GetTile(i, j).GetComponent<Tile>().isWall == false)
                return true;

        return false;
    }

    /// <summary>
    /// 지정한 좌표에 몬스터가 존재하면 true 를 반환합니다.
    /// </summary>
    public static bool CheckMob(int i, int j)
    {
        bool check = false;

        foreach (Mob mob in Mob.Mobs)
        {
            if (mob.i == i && mob.j == j)
            {
                check = true;
            }
        }

        return check;
    }

    /// <summary>
    /// <br>두 타일간의 거리를 반환합니다.</br>
    /// </summary>
    public static int GetDistance(Tile a, Tile b, RangeType rangeType)
    {
        switch (rangeType)
        {
            case RangeType.Manhattan:
                return Math.Abs(a.i - b.i) + Math.Abs(a.j - b.j);
            case RangeType.Chebyshev:
                return Math.Max(Math.Abs(a.i - b.i), Math.Abs(a.j - b.j));
            default:
                return GetDistance(a, b, RangeType.Manhattan);
        }
    }

    /// <summary>
    /// 지정한 행렬의 타일에 접근하여 isWall 속성을 뒤집습니다.
    /// </summary>
    public static void SwitchWall(int i, int j)
    {
        Tile tile = Grid.GetTile(i, j).GetComponent<Tile>();

        tile.isWall = !tile.isWall;
    }
}