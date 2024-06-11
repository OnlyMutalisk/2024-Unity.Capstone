using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public Canvas canv;
    public GridLayoutGroup grid;
    public static GameObject obj;
    public static float cellSize;
    public static float spacing;

    // 행의 개수 i, 열의 개수 j
    public static int i, j;
        
    private void Awake()
    {
        Grid.obj = gameObject;
        cellSize = grid.cellSize.x;
        spacing = grid.spacing.x;
        i = (int)((canv.GetComponent<RectTransform>().rect.height + spacing) / (cellSize + spacing));
        j = (int)((canv.GetComponent<RectTransform>().rect.width + spacing) / (cellSize + spacing));
    }

    // 타일 임시 초기화
    public void Start()
    {
        for (int i = 10; i < 30; i++)
        {
            for (int j = 19; j < 38; j++)
            {
                GetTile(i, j).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Tile_Normal");
                Color a = GetTile(i, j).GetComponent<Image>().color;
                a.a = 1f;
                GetTile(i, j).GetComponent<Image>().color = a;
            }
        }
    }

    /// <summary>
    /// <br>[i][j] 을 인덱스화 합니다.</br>
    /// </summary>
    public static int ConvertArrayToIndex(int i, int j)
    {
        int index = Grid.j * (i - 1) + j - 1;

        return index;
    }

    /// <summary>
    /// <br>인덱스를 [i][j] 로 변경합니다.</br>
    /// </summary>
    public static (int i, int j) ConvertIndexToArray(int index)
    {
        // 행과 열 인덱스 계산
        int i = index / Grid.j + 1;
        int j = index % Grid.j + 1;

        return (i, j);
    }

    /// <summary>
    /// <br>타일 [i][j] 을 반환합니다.</br>
    /// </summary>
    public static GameObject GetTile(int i, int j)
    {
        return obj.transform.GetChild(ConvertArrayToIndex(i, j)).gameObject;
    }
}
