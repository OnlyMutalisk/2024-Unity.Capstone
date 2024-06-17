using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Mob
{
    public override int i { get; set; } = Player.i;
    public override int j { get; set; } = Player.j;
    public override float HP { get; set; } = GameManager.HP_Bishop;
    public override float HP_max { get; set; } = GameManager.HP_Bishop;
    public override int damage { get; set; } = GameManager.damage_Bishop;
    public override int maxAction { get; set; } = GameManager.action_Bishop;
    public override int action { get; set; } = GameManager.action_Bishop;
    public override int range { get; set; } = GameManager.range_Bishop;
    public override int attackCost { get; set; } = GameManager.attackCost_Bishop;
    public override int moveCost { get; set; } = GameManager.moveCost_Bishop;
    public override int visionRange { get; set; } = GameManager.visionRange_Bishop;
    public override string moveType { get; set; } = "Bishop";
}
