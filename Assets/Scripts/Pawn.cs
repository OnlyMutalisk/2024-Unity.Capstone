using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Mob
{
    public override float HP { get; set; } = 10;
    public override int i { get; set; } = Player.i;
    public override int j { get; set; } = Player.j;

    // Update is called once per frame
    void Update()
    {
        HP_slider.value = HP / GameManager.HP_Pawn;
    }
}
