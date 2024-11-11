using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public static class GameManager
{
    public enum PlayerAction { Move, Attack, Skill };
    public static PlayerAction playerAction = PlayerAction.Move;
    public static Dictionary<string, string> tileProperty = new Dictionary<string, string>();
    public static Dictionary<string, string> propertyCounterMatch = new Dictionary<string, string>();

    // 전장의 안개 (Fog) 를 관리합니다. (밝음 0 ~ 1 어두움)
    public static int visionRange = 2;
    public static float Gamma_unvisited = 1f;
    public static float Gamma_vision = 0f;
    public static float Gamma_visited = 0.8f;

    // 타일의 감마를 조절합니다.
    public static float Gamma_Tile = 1f;
    public static float Gamma_Tile_Empty = 0.031f;

    // 딜레이 및 속도를 관리합니다.
    public static float speed_Char = 2f;
    public static float speed_Mob = 1.5f;
    public static float delay_loading = 3f;
    public static float delay_mobMove = 0.2f;
    public static float delay_msgTopAction = 2f;
    public static float delay_mobDestroy = 3f;

    // 캐릭터의 스텟을 조정합니다.
    public static int action_Char = 10;
    public static float attackDamage_Char = 5;
    public static float skillDamage_Char = 3;
    public static int attackDistance_Char = 1;
    public static int skillDistance_Char = 3;
    public static int Life_Char = 10;
    public static int shield_Char = 4;

    // 캐릭터 행동력 비용입니다.
    public static int cost_Pawn = 1;
    public static int cost_Knight = 2;
    public static int cost_Bishop = 2;
    public static int cost_Rook = 2;
    public static int cost_Attack = 3;
    public static int cost_Skill = 4;

    // Pawn 의 스텟을 조정합니다.
    public static float HP_Pawn = 10;
    public static int damage_Pawn = 2;
    public static int action_Pawn = 3;
    public static int range_Pawn = 1;
    public static int attackCost_Pawn = 1;
    public static int moveCost_Pawn = 1;
    public static int visionRange_Pawn = 2;

    // Knight 의 스텟을 조정합니다.
    public static float HP_Knight = 10;
    public static int damage_Knight = 1;
    public static int action_Knight = 2;
    public static int range_Knight = 2;
    public static int attackCost_Knight = 1;
    public static int moveCost_Knight = 1;
    public static int visionRange_Knight = 3;

    // Bishop 의 스텟을 조정합니다.
    public static float HP_Bishop = 10;
    public static int damage_Bishop = 1;
    public static int action_Bishop = 4;
    public static int range_Bishop = 1;
    public static int attackCost_Bishop = 1;
    public static int moveCost_Bishop = 1;
    public static int visionRange_Bishop = 2;

    // Item_Shield 의 효과를 조정합니다.
    public static int recovery_shield = 4;

    // Message_Top
    public static string msg_turn = "상대방의 턴 입니다...";
    public static string msg_loading = "맵 로딩 중 입니다...";
    public static string msg_action = "행동력이 부족합니다. 다른 행동을 하거나, 배터리를 눌러 턴을 종료해주세요.";

    // Message_Move
    public static string msg_Pawn = $"타일 당 소요 행동력 : {cost_Pawn}";
    public static string msg_Knight = $"타일 당 소요 행동력 : {cost_Knight}";
    public static string msg_Bishop = $"타일 당 소요 행동력 : {cost_Bishop}";
    public static string msg_Rook = $"타일 당 소요 행동력 : {cost_Rook}";
    public static string msg_Attack = $"공격 소요 행동력 : {cost_Attack}\n공격력 : {attackDamage_Char}";
    public static string msg_Skill = $"스킬 소요 행동력 : {cost_Skill}\n공격력 : {skillDamage_Char}";

    // 타일 상성 보너스 데미지 상수 입니다.
    public static float propertyBonus = 2f;

    static GameManager()
    {
        // 여기에 타일 상성을 추가합니다. 전자가 상성 우위입니다.
        propertyCounterMatch.Add("Tile_Forest", "Tile_Water");
        propertyCounterMatch.Add("Tile_Water", "Tile_Ground");
        propertyCounterMatch.Add("Tile_Ground", "Tile_Forest");

        tileProperty.Add("Tile_Empty", "빈 타일");
        tileProperty.Add("Tile_Normal", "일반 타일");
        tileProperty.Add("Tile_Forest", "숲\n(물 타일 데미지 보너스 x2)");
        tileProperty.Add("Tile_Water", "물\n(땅 타일 데미지 보너스 x2)");
        tileProperty.Add("Tile_Ground", "땅\n(숲 타일 데미지 보너스 x2)");
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