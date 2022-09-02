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

    //�u���b�N�I�u�W�F�N�g�̖��O����u���b�N�̏ꏊ�̃C���f�b�N�X���擾����
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

    //�C���f�b�N�X����boardAddress���Q�Ƃ���transform.position�̃x�N�g����Ԃ�
    public static Vector3 GetPositionFromIndex(int[] index)
    {
        Vector3 pos;

        pos.x = (float)boardAddress[index[0], index[1], 0];
        pos.y = (float)boardAddress[index[0], index[1], 1];
        pos.z = 0f;

        return pos;
    }

    // �C���f�b�N�X����u���b�N�̃I�u�W�F�N�g����Ԃ�
    public static string GetBlockNameFromIndex(int[] index)
    {
        string blockName;

        blockName = NameDefinition.BLOCK + index[0].ToString("0") + index[1].ToString("0");

        return blockName;
    }

    // ���W�i��j���擾
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
