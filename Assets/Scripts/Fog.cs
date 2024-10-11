using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    public Transform grid;
    private List<SpriteRenderer> fogs = new List<SpriteRenderer>();
    private List<SpriteRenderer> visitedfogs = new List<SpriteRenderer>();
    private int visionRange = GameManager.visionRange;
    private float Gamma_unvisited = GameManager.Gamma_unvisited;
    private float Gamma_vision = GameManager.Gamma_vision;
    private float Gamma_visited = GameManager.Gamma_visited;

    void Start()
    {
        AddFogs();

        ResizeFogs();
    }

    private void Update()
    {
        UpdateFog();
    }

    /// <summary>
    /// 모든 Fog 를 추가합니다.
    /// </summary>
    private void AddFogs()
    {
        foreach (Transform tile in grid)
        {
            fogs.Add(tile.GetChild(0).GetComponent<SpriteRenderer>());
        }
    }

    /// <summary>
    /// Fog 들의 스프라이트 크기를 Grid.cellSize + Grid.spacing 으로 설정합니다.
    /// </summary>
    private void ResizeFogs()
    {
        float targetWidth = Grid.cellSize + Grid.spacing;
        float targetHeight = Grid.cellSize + Grid.spacing;

        foreach (SpriteRenderer fog in fogs)
        {
            // 스프라이트를 RectTransform 크기에 맞추는 비율 계산
            Vector3 newScale = fog.gameObject.transform.localScale;
            newScale.x = targetWidth / fog.bounds.size.x;
            newScale.y = targetHeight / fog.bounds.size.y;

            // 스프라이트 크기 조정
            fog.gameObject.transform.localScale = newScale;
        }
    }

    /// <summary>
    /// 플레이어 주변 Fog 를 밝히고, 멀어진 Fog 를 어둡게 하는 Fog Update 함수입니다.
    /// </summary>
    private void UpdateFog()
    {
        // 리스트의 fog (방문했던 fog) 감마를 어둡게 합니다.
        foreach (SpriteRenderer fog in visitedfogs)
        {
            Color tempColor = fog.color;
            tempColor.a = Gamma_visited;
            fog.color = tempColor;
        }

        // 리스트를 초기화합니다.
        visitedfogs.Clear();

        // 플레이어 주변 fog 를 캐치해 리스트에 담습니다.
        for (int i = -visionRange; i <= visionRange; i++)
        {
            for (int j = -visionRange; j <= visionRange; j++)
            {
                // 타일이 맵 안쪽일 경우에만 리스트에 담습니다.
                if (Player.i + i <= Grid.i && Player.j + j <= Grid.j == true)
                {
                    visitedfogs.Add(Grid.GetTile(Player.i + i, Player.j + j).transform.GetChild(0).GetComponent<SpriteRenderer>());
                }
            }
        }

        // 시야범위의 모퉁이를 다듬습니다.
        if (Player.i + visionRange <= Grid.i && Player.j + visionRange <= Grid.j)
            visitedfogs.Remove(Grid.GetTile(Player.i + visionRange, Player.j + visionRange).transform.GetChild(0).GetComponent<SpriteRenderer>());
        if (Player.i + visionRange <= Grid.i && Player.j - visionRange <= Grid.j)
            visitedfogs.Remove(Grid.GetTile(Player.i + visionRange, Player.j - visionRange).transform.GetChild(0).GetComponent<SpriteRenderer>());
        if (Player.i - visionRange <= Grid.i && Player.j + visionRange <= Grid.j)
            visitedfogs.Remove(Grid.GetTile(Player.i - visionRange, Player.j + visionRange).transform.GetChild(0).GetComponent<SpriteRenderer>());
        if (Player.i - visionRange <= Grid.i && Player.j - visionRange <= Grid.j)
            visitedfogs.Remove(Grid.GetTile(Player.i - visionRange, Player.j - visionRange).transform.GetChild(0).GetComponent<SpriteRenderer>());


        // 리스트의 fog (시야 내의 fog) 감마를 밝게 합니다.
        foreach (SpriteRenderer fog in visitedfogs)
        {
            fog.color = fog.color * new Color(1, 1, 1, Gamma_vision);
        }
    }

    /// <summary>
    /// 모든 Fog 를 초기화합니다.
    /// </summary>
    private void ResetFog()
    {
        // 모든 fog 의 감마를 1로 초기화 합니다.
        foreach (SpriteRenderer fog in fogs)
        {
            fog.color = fog.color * new Color(1, 1, 1, Gamma_unvisited);
        }

        // 리스트를 비웁니다.
        fogs.Clear();
    }
}