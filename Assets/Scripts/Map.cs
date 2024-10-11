using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public GameObject canvas; // 하이라키 오브젝트 연결
    public GameObject user; // 하이라키 오브젝트 연결
    public GameObject enemy_pawn; // 프리팹
    public GameObject enemy_knight; // 프리팹
    public GameObject enemy_bishop; // 프리팹
    public GameObject item_shield; // 프리팹
    public static int index;
    private Dictionary<string, string> ColorToTile = new Dictionary<string, string>();
    private Dictionary<string, GameObject> TextToUnit = new Dictionary<string, GameObject>();

    private void Start()
    {
        ColorToTile.Add("FFA162D0", "Tile_Empty");
        ColorToTile.Add("FFFDE7FB", "Tile_Normal");
        ColorToTile.Add("FF6ED749", "Tile_Forest");
        ColorToTile.Add("FF3FCDFF", "Tile_Water");
        ColorToTile.Add("FF744A0C", "Tile_Ground");

        TextToUnit.Add("U", user);
        TextToUnit.Add("E_P", enemy_pawn);
        TextToUnit.Add("E_N", enemy_knight);
        TextToUnit.Add("E_B", enemy_bishop);
        TextToUnit.Add("I_S", item_shield);

        LoadMap("Map.xlsx", index);
    }

    /// <summary>
    /// Resources 폴더의 xlsx 파일을 읽어, 맵을 로드합니다.
    /// </summary>
    private void LoadMap(string fileName, int index)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath))
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[index];
                int rows = Grid.i + 1;
                int columns = Grid.j + 1;

                for (int row = 2; row <= rows; row++)
                {
                    for (int col = 2; col <= columns; col++)
                    {
                        ExcelRange cell = worksheet.Cells[row, col];
                        var cellValue = cell.Text;
                        var cellColor = cell.Style.Fill.BackgroundColor;
                        Debug.Log($"Cell ({row - 1}, {col - 1}) Value: {cellValue}, Color: {cellColor.Rgb}");
                    }
                }

                for (int row = 2; row <= rows; row++)
                {
                    for (int col = 2; col <= columns; col++)
                    {
                        ExcelRange cell = worksheet.Cells[row, col];
                        var cellValue = cell.Text;
                        var cellColor = cell.Style.Fill.BackgroundColor;

                        Debug.Log($"Cell ({row - 1}, {col - 1}) Value: {cellValue}, Color: {cellColor.Rgb}");

                        // 16진수 컬러 값에 따라 타일의 감마, 스프라이트 교체
                        GameObject tile = Grid.GetTile(row - 1, col - 1);
                        Image img = tile.GetComponent<Image>();
                        img.color = new UnityEngine.Color(255, 255, 255, GameManager.Gamma_Tile);
                        img.sprite = Resources.Load<Sprite>("Images/" + ColorToTile[cellColor.Rgb]);

                        // Tile_Empty 처리
                        if (ColorToTile[cellColor.Rgb] == "Tile_Empty")
                        {
                            img.color = new UnityEngine.Color(255, 255, 255, GameManager.Gamma_Tile_Empty);
                            tile.GetComponent<Tile>().isWall = true;
                        }

                        // 타일 위치로 유저 이동
                        if (cellValue == "U") { StartCoroutine(Player.CorMove(row - 1, col - 1)); }

                        // 타일 위치에 몬스터 생성
                        if (cellValue.StartsWith("E"))
                        {
                            GameObject mob = Instantiate(TextToUnit[cellValue], new Vector3(0, 0, 0), Quaternion.identity, canvas.transform);
                            Mob script = mob.GetComponent<Mob>();
                            script.i = (Grid.i / 2) + 1;
                            script.j = (Grid.j / 2) + 1;
                            StartCoroutine(script.CorMove(row - 1, col - 1));
                        }

                        // 타일 위치로 아이템 이동
                        if (cellValue.StartsWith("I"))
                        {
                            GameObject item = Instantiate(TextToUnit[cellValue], new Vector3(0, 0, 0), Quaternion.identity, canvas.transform);
                            Item script = item.GetComponent<Item>();
                            script.sprite = item.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                            Item.Items.Add(script);
                            script.i = (Grid.i / 2) + 1;
                            script.j = (Grid.j / 2) + 1;
                            StartCoroutine(script.CorMove(row - 1, col - 1));
                        }
                    }
                }
            }
        }
        else { Debug.LogError(filePath + " 경로에 파일이 존재하지 않습니다."); }
    }
}