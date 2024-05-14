using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;


public static class GameManager
{
    // 캐릭터의 스텟을 조정합니다.
    public static float speed_Char = 1f;
    public static int maxAction_Char = 10; // 초기 및 최대 행동력
    public static float damage_Char = 1;
    public static int attackDistance_Char = 1;

    // 캐릭터 행동력 비용입니다.
    public static int cost_Pawn = 1;
    public static int cost_Knight = 2;
    public static int cost_Bishop = 2;
    public static int cost_Rook = 2;
    public static int cost_Queen = 3;
    public static int cost_Attack = 5;

    // 몬스터의 스텟을 조정합니다.
    public static float HP_Pawn = 10;

    public enum PlayerAction { Move, Attack, Skill };
    public enum TileProperty { Ground, Water, Forest, Fire };
    public static PlayerAction playerAction = PlayerAction.Move;
    public static TileProperty playerTileProperty = TileProperty.Ground;

    #region JSON SAVE & LOAD

    public static string savedDataPath;

    public static void Awake()
    {
        savedDataPath = Path.Combine(Application.persistentDataPath, "data.json");
    }

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

#endregion