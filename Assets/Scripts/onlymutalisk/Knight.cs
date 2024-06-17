using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Mob
{
    public override int i { get; set; } = Player.i;
    public override int j { get; set; } = Player.j;
    public override float HP { get; set; } = GameManager.HP_Knight;
    public override float HP_max { get; set; } = GameManager.HP_Knight;
    public override int damage { get; set; } = GameManager.damage_Knight;
    public override int maxAction { get; set; } = GameManager.action_Knight;
    public override int action { get; set; } = GameManager.action_Knight;
    public override int range { get; set; } = GameManager.range_Knight;
    public override int attackCost { get; set; } = GameManager.attackCost_Knight;
    public override int moveCost { get; set; } = GameManager.moveCost_Knight;
    public override int visionRange { get; set; } = GameManager.visionRange_Knight;
    public override string moveType { get; set; } = "Knight";
}
