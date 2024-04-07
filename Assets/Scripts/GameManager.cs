using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum PlayerAction { Move, Attack, Skill };
    public enum TileProperty { Ground, Water, Forest, Fire };

    public static PlayerAction playerAction = PlayerAction.Move;
    public static TileProperty playerTileProperty = TileProperty.Ground;
}
