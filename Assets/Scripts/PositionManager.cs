using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class PositionManager
{
    private static readonly int[,,] boardAddress = new int[5, 5, 2]
    {
        { {-2, 2}, {-2, 1}, {-2, 0}, {-2, -1}, {-2, -2} },
        { {-1, 2}, {-1, 1}, {-1, 0}, {-1, -1}, {-1, -2} },
        { {0, 2}, {0, 1}, {0, 0}, {0, -1}, {0, -2} },
        { {1, 2}, {1, 1}, {1, 0}, {1, -1}, {1, -2} },
        { {2, 2}, {2, 1}, {2, 0}, {2, -1}, {2, -2} }
    };

    public static int INITPOSITION = 8;
    public static string NOTSET = "not setted";

    //ブロックオブジェクトの名前からブロックの場所のインデックスを取得する
    public static int[] GetAddressFromGameObjectName(string gameObjectName)
    {
        int x;
        int y;

        int[] address = new int[2];

        x = int.Parse(gameObjectName.Substring(gameObjectName.Length - 2, 1));
        y = int.Parse(gameObjectName.Substring(gameObjectName.Length - 1, 1));

        address[0] = x;
        address[1] = y;

        return address;
    }

    //インデックスからboardAddressを参照してtransform.positionのベクトルを返す
    public static Vector3 GetPositionFromIndex(int[] index)
    {
        Vector3 pos;

        pos.x = (float)boardAddress[index[0], index[1], 0];
        pos.y = (float)boardAddress[index[0], index[1], 1];
        pos.z = 0f;

        return pos;
    }

    // インデックスからブロックのオブジェクト名を返す
    public static string GetBlockNameFromIndex(int[] index)
    {
        string blockName;

        blockName = NameDefinition.BLOCK + index[0].ToString("0") + index[1].ToString("0");

        return blockName;
    }

    // 座標（列）を取得
    public static string GetChartAlphabet(int index)
    {
        string txt;
        switch (index)
        {
            case 0:
                txt = "A";
                break;
            case 1:
                txt = "B";
                break;
            case 2:
                txt = "C";
                break;
            case 3:
                txt = "D";
                break;
            case 4:
                txt = "E";
                break;
            default:
                txt = "";
                break;
        }

        return txt;
    }
}
