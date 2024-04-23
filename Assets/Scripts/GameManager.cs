using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    public enum PlayerAction { Move, Attack, Skill };
    public enum TileProperty { Ground, Water, Forest, Fire };

    public static PlayerAction playerAction = PlayerAction.Move;
    public static TileProperty playerTileProperty = TileProperty.Ground;

    public RectTransform backGround;
    public static float tileSize_diameter;
    public static float tileSize_diagonal;

    private void Awake()
    {
        tileSize_diameter = backGround.sizeDelta.x / 100;
        tileSize_diagonal = backGround.sizeDelta.y / 100;
    }

    #region JSON SAVE & LOAD

    public static string savedDataPath = Path.Combine(Application.persistentDataPath, "data.json");

    public static void SAVE()
    {
        Data data = new Data
        {
            // 데이터 틀에 현재 데이터들을 담습니다.
            playerAction = GameManager.playerAction,
            playerTileProperty = GameManager.playerTileProperty
        };

        // 데이터를 JSON 으로 직렬화합니다.
        string jsonData = JsonConvert.SerializeObject(data);

        // 직렬화한 데이터를 파일로 저장합니다.
        File.WriteAllText(savedDataPath, jsonData);
    }

    public static void LOAD()
    {
        // 파일로 저장한 데이터를 직렬화 된 상태로 가져옵니다.
        string jsonData = File.ReadAllText(savedDataPath);

        // 직렬화를 풀어 데이터 틀에 담습니다.
        Data data = JsonConvert.DeserializeObject<Data>(jsonData);

        // 현재 데이터로 덮어씁니다.
        playerAction = data.playerAction;
        playerTileProperty = data.playerTileProperty;
    }

    public static void RESET()
    {
        playerAction = 0;
        playerTileProperty = 0;
    }

}

// 데이터 틀 입니다.
public class Data
{
    public GameManager.PlayerAction playerAction;
    public GameManager.TileProperty playerTileProperty;
}

#endregion//