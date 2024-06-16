using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public string xlsxPath = "C:\\Users\\Onlym\\Downloads\\test.xlsx";
    private Dictionary<string, string> ColorToTile = new Dictionary<string, string>();
    private Dictionary<string, string> TextToTile = new Dictionary<string, string>();

    private void Start()
    {
        ColorToTile.Add("FFA162D0", "Tile_Empty");
        ColorToTile.Add("FFFDE7FB", "Tile_Normal");

        LoadMap("Map.xlsx");
    }

    /// <summary>
    /// Resources 폴더의 xlsx 파일을 읽어, 맵을 로드합니다.
    /// </summary>
    private void LoadMap(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath))
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
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

                        // 16진수 컬러 값에 따라 타일의 감마, 스프라이트 교체
                        GameObject tile = Grid.GetTile(row - 1, col - 1);
                        Image img = tile.GetComponent<Image>();
                        img.color = new UnityEngine.Color(255, 255, 255, 1);
                        img.sprite = Resources.Load<Sprite>("Images/" + ColorToTile[cellColor.Rgb]);

                        // Tile_Empty 처리
                        if (ColorToTile[cellColor.Rgb] == "Tile_Empty")
                        {
                            img.color = new UnityEngine.Color(255, 255, 255, 0.031f);
                            tile.GetComponent<Tile>().isWall = true;
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogError(xlsxPath + " 경로에 파일이 존재하지 않습니다.");
        }
    }
}