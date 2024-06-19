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
    public enum PlayerAction { Move, Attack, Skill };
    public static PlayerAction playerAction = PlayerAction.Move;
    public static Dictionary<string, string> tileProperty = new Dictionary<string, string>();

    // 캐릭터의 스텟을 조정합니다.
    public static float speed_Char = 1f;
    public static int action_Char = 10;
    public static float damage_Char = 1;
    public static int attackDistance_Char = 1;
    public static int skillDistance_Char = 3;
    public static int Life_Char = 10;

    // 캐릭터 행동력 비용입니다.
    public static int cost_Pawn = 1;
    public static int cost_Knight = 2;
    public static int cost_Bishop = 2;
    public static int cost_Rook = 2;
    public static int cost_Attack = 5;
    public static int cost_Skill = 10;

    // Pawn 의 스텟을 조정합니다.
    public static float HP_Pawn = 10;
    public static int damage_Pawn = 1;
    public static int action_Pawn = 10;
    public static int range_Pawn = 1;
    public static int attackCost_Pawn = 5;
    public static int moveCost_Pawn = 2;
    public static int visionRange_Pawn = 2;

    // Knight 의 스텟을 조정합니다.
    public static float HP_Knight = 10;
    public static int damage_Knight = 1;
    public static int action_Knight = 10;
    public static int range_Knight = 1;
    public static int attackCost_Knight = 5;
    public static int moveCost_Knight = 2;
    public static int visionRange_Knight = 3;

    // Bishop 의 스텟을 조정합니다.
    public static float HP_Bishop = 10;
    public static int damage_Bishop = 1;
    public static int action_Bishop = 10;
    public static int range_Bishop = 1;
    public static int attackCost_Bishop = 5;
    public static int moveCost_Bishop = 2;
    public static int visionRange_Bishop = 2;

    // Message_Top
    public static string msg_turn = "It's the opponent's turn...";
    public static string msg_loading = "Loading Map....";
    public static float delay_loading = 5f;

    // Message_Move
    public static string msg_Pawn = "타일 당 소요 행동력 : " + cost_Pawn;
    public static string msg_Knight = "타일 당 소요 행동력 : " + cost_Knight;
    public static string msg_Bishop = "타일 당 소요 행동력 : " + cost_Bishop;
    public static string msg_Rook = "타일 당 소요 행동력 : " + cost_Rook;
    public static string msg_Attack = "공격 소요 행동력 : " + cost_Attack;
    public static string msg_Skill = "스킬 소요 행동력 : " + cost_Skill;

    static GameManager()
    {
        tileProperty.Add("Tile_Empty", "빈 타일");
        tileProperty.Add("Tile_Normal", "일반 타일");
        tileProperty.Add("Tile_Forest", "숲 타일 (물 타일 추가 피해 +50%)");
        tileProperty.Add("Tile_Water", "물 타일 (땅 타일 추가 피해 +50%)");
        tileProperty.Add("Tile_Ground", "땅 타일 (숲 타일 추가 피해 +50%)");
        tileProperty.Add("Tile_Select", "이동 가능한 타일");
        tileProperty.Add("Tile_Select_Attack", "공격 범위 내 타일");
        tileProperty.Add("Tile_Select_Skill", "스킬 범위 내 타일");
    }

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
    }

    public static void RESET()
    {
        playerAction = 0;
    }

}

// 데이터 틀 입니다.
public class Data
{
    public GameManager.PlayerAction playerAction;
}

#endregion