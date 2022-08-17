using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NameDefinition
{
    public static string BLOCK = "block";
    public static string CHART = "chart";
    public static string BATTLESHIP = "BattleShip";
    public static string DESTROYER = "Destroyer";
    public static string SUBMARINE = "Submarine";

    public static readonly string[] SHIPLIST = new string[3]
    {
        BATTLESHIP,
        DESTROYER,
        SUBMARINE
    };

    public static string NOTSET = "NotSet";

    public static string BLOCKTAG = "Untagged";
    public static string PLAYERTAG = "Player";
    public static string SHIPSTAG = "Ships";
}
