using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Mob
{
    public override int i { get; set; } = Player.i;
    public override int j { get; set; } = Player.j;
    public override float HP { get; set; } = GameManager.HP_Pawn;
    public override int damage { get; set; } = GameManager.damage_Pawn;
    public override int maxAction { get; set; } = GameManager.action_Pawn;
    public override int action { get; set; } = GameManager.action_Pawn;
    public override int range { get; set; } = GameManager.range_Pawn;
    public override int attackCost { get; set; } = GameManager.attackCost_Pawn;
    public override int moveCost { get; set; } = GameManager.moveCost_Pawn;
    public override string moveType { get; set; } = "Pawn";

    // Update is called once per frame
    void Update()
    {
        HP_slider.value = HP / GameManager.HP_Pawn;
    }
}
