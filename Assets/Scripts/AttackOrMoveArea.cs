using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackOrMoveArea : MonoBehaviour
{
    private const int boardSize = 5;
    private GameObject battleshipObj;
    public int[] notsetIdx = new int[2];
    private string notsetObjName;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < notsetIdx.Length; i++)
        {
            notsetIdx[i] = PositionManager.INITPOSITION;
        }
        notsetObjName = PositionManager.GetBlockNameFromIndex(notsetIdx);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 攻撃・移動範囲データを取得
    public bool SetAttackOrMoveAreaIndex(GamePlayer player, int[] index1, int[] index2, int[] index3)
    {
        bool isArea = true;
        // 攻撃範囲を取得
        player.attackAreaBlockNameList = GetAttackArea(index1, index2, index3);

        // 移動範囲の取得
        if (player.selectShipName == NameDefinition.BATTLESHIP)
        {
            player.moveAreaBlockNameList = GetMoveAreaIndex(index1, index2, index3);
        }
        else if (player.selectShipName == NameDefinition.DESTROYER)
        {
            player.moveAreaBlockNameList = GetMoveAreaIndex(index2, index1, index3);
        }
        else if (player.selectShipName == NameDefinition.SUBMARINE)
        {
            player.moveAreaBlockNameList = GetMoveAreaIndex(index3, index1, index2);
        }
        else
        {
            isArea = false;
        }

        return isArea;
    }

    // 攻撃範囲を初期化
    public void InitAttackAreaIndex(GamePlayer player)
    {
        for (int i = 0; i < player.attackAreaBlockNameList.Length; i++)
        {
            player.attackAreaBlockNameList[i] = PositionManager.NOTSET;
        }
    }

    // 移動範囲を初期化
    public void InitMoveAreaIndex(GamePlayer player)
    {
        for (int i = 0; i < player.moveAreaBlockNameList.Length; i++)
        {
            player.moveAreaBlockNameList[i] = PositionManager.NOTSET;
        }
        // 移動対象の戦艦をリセット
        player.selectShipName = NameDefinition.NOTSET;
    }

    // HPが0になった船を破壊する
    public void DestroyShips(GamePlayer player, string shipName)
    {
        GameObject gameObject = GameObject.Find(shipName);

        if (shipName == NameDefinition.BATTLESHIP)  // 戦艦を破壊
        {
            for (int i = 0; i < player.battleshipIdx.Length; i++)
            {
                player.battleshipIdx[i] = PositionManager.INITPOSITION;
            }
            Destroy(gameObject);
        }
        else if (shipName == NameDefinition.DESTROYER)  // 駆逐艦を破壊
        {
            for (int i = 0; i < player.destroyerIdx.Length; i++)
            {
                player.destroyerIdx[i] = PositionManager.INITPOSITION;
            }
            Destroy(gameObject);
        }
        else if (shipName == NameDefinition.SUBMARINE)  // 潜水艦を破壊
        {
            for (int i = 0; i < player.submarineIdx.Length; i++)
            {
                player.submarineIdx[i] = PositionManager.INITPOSITION;
            }
            Destroy(gameObject);
        }
    }

    // 攻撃範囲をすべて取得
    public string[] GetAttackArea(int[] index1, int[] index2, int[] index3)
    {
        string[] attackAreaList = new string[27];
        attackAreaList = GetAllAttackAreaIndex(attackAreaList, index1, 0);
        attackAreaList = GetAllAttackAreaIndex(attackAreaList, index2, 1);
        attackAreaList = GetAllAttackAreaIndex(attackAreaList, index3, 2);

        return attackAreaList;
    }

    // 攻撃範囲の重複を解消
    private string[] GetAllAttackAreaIndex(string[] attackList, int[] index, int offset)
    {
        string[] newAttackList = attackList;
        string target = PositionManager.GetBlockNameFromIndex(index);
        if (target != notsetObjName)
        {
            string[] nameList = new string[9];
            nameList = GetAttackAreaIndex(index);
            for (int j = 0; j < nameList.Length; j++)
            {
                if (nameList[j] != null)
                {
                    if (Array.IndexOf(newAttackList, nameList[j]) == -1)
                    {
                        newAttackList[j + offset * 9] = nameList[j];
                    }
                    else
                    {
                        newAttackList[j + offset * 9] = PositionManager.NOTSET;
                    }
                }
                else
                {
                    newAttackList[j + offset * 9] = PositionManager.NOTSET;
                }
            }
        }
        return newAttackList;
    }
    
    public bool IsInList(string[] list, string element)
    {
        bool isInList = false;
        if (Array.IndexOf(list, element) != -1)
        {
            isInList = true;
        }

        return isInList;
    }

    // 戦艦ごとの攻撃範囲を取得
    public string[] GetAttackAreaIndex(int[] index)
    {
        int count = 0;
        string[] attackAreaBlockName = new string[9];

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int x = index[0] + (i - 1);
                int y = index[1] + (j - 1);
                if (x >= 0 && x < boardSize && y >= 0 && y < boardSize)
                {
                    attackAreaBlockName[count] = NameDefinition.BLOCK + x.ToString("0") + y.ToString("0");
                    count++;
                }
            }
        }
        
        return attackAreaBlockName;
    }

    // 移動範囲を取得
    private string[] GetMoveAreaIndex(int[] index, int[] otherIndex1, int[] otherIndex2)
    {
        int count = 0;
        string[] moveAreaBlockName = new string[9];

        for (int i = 0; i < 5; i++)
        {
            if (i != index[1])
            {
                int x = index[0];
                int y = i;
                int[] tmp_index = { x, y };
                UnityEngine.Debug.Log(tmp_index[0]);
                if (JudgeEqual(tmp_index, otherIndex1) || JudgeEqual(tmp_index, otherIndex2))
                {
                    moveAreaBlockName[count] = PositionManager.NOTSET;
                }
                else
                {
                    moveAreaBlockName[count] = NameDefinition.BLOCK + x.ToString("0") + y.ToString("0");
                }
                //moveAreaBlockName[count] = NameDefinition.BLOCK + x.ToString("0") + y.ToString("0");
                count++;
            }
            if (i != index[0])
            {
                int x = i;
                int y = index[1];
                int[] tmp_index = { x, y };
                UnityEngine.Debug.Log(tmp_index[0]);
                if (JudgeEqual(tmp_index, otherIndex1) || JudgeEqual(tmp_index, otherIndex2))
                {
                    moveAreaBlockName[count] = PositionManager.NOTSET;
                }
                else
                {
                    moveAreaBlockName[count] = NameDefinition.BLOCK + x.ToString("0") + y.ToString("0");
                }
                //moveAreaBlockName[count] = NameDefinition.BLOCK + x.ToString("0") + y.ToString("0");
                count++;
            }
        }

        return moveAreaBlockName;
    }

    // 移動を選択時のログを取得
    public string GetMoveLog(DeployShips deployShips, GamePlayer player)
    {
        string logText;

        string[] shipDataList = new string[3];
        shipDataList = deployShips.MoveShip(player);

        int[] before = new int[2];
        int[] after = new int[2];
        before = PositionManager.GetAddressFromGameObjectName(shipDataList[1]);
        after = PositionManager.GetAddressFromGameObjectName(shipDataList[2]);

        int dif_x = after[0] - before[0];
        int dif_y = after[1] - before[1];
        int[] moveDif_x = new int[2];
        int[] moveDif_y = new int[2];
        moveDif_x = deployShips.GetMoveDifference(dif_x);
        moveDif_y = deployShips.GetMoveDifference(dif_y);

        string direction;
        string distance;

        if (moveDif_x[0] != 0)
        {
            if (moveDif_x[0] > 0)
            {
#if UNITY_WEBGL
                direction = "\"to the right\"";
#else
                direction = "\"右\"";
#endif
            }
            else
            {
#if UNITY_WEBGL
                direction = "\"to the left\"";
#else
                direction = "\"左\"";
#endif
            }
            distance = moveDif_x[1].ToString("0");
        }
        else
        {
            if (moveDif_y[0] > 0)
            {
#if UNITY_WEBGL
                direction = "\"downwards\"";
#else
                direction = "\"下\"";
#endif
            }
            else
            {
#if UNITY_WEBGL
                direction = "\"upwards\"";
#else
                direction = "\"上\"";
#endif
            }
            distance = moveDif_y[1].ToString("0");
        }

#if UNITY_WEBGL
        logText = "  " + player.playerName + " : "
            + shipDataList[0] + " moved "
            + distance + " block "
            + direction;
#else
        logText = "  " + player.playerName + " : \""
            + shipDataList[0] + "\" を "
            + direction + " に "
            + distance + " ブロック移動しました ! ";
#endif

        return logText;
    }

    // 攻撃が立った場合のログを取得
    public string GetHitLog(int[] Index, string playerName, int num)
    {
        string logText;

        string col = PositionManager.GetChartAlphabet(Index[1]);
        int row = Index[0] + 1;

#if UNITY_WEBGL
        logText = "  " + playerName + " : "
            + "Attacked \""
            + row.ToString("0") + col + "\", and hit the \""
            + NameDefinition.SHIPLIST[num]
            + "\"!";
#else
        logText = "  " + playerName + " : "
            + "\""
            + row.ToString("0") + col + "\" を攻撃し、\""
            + NameDefinition.SHIPLIST[num]
            + "\" に当たりました ! ";
#endif

        return logText;
    }

    // 攻撃がニアミスの場合のログを取得
    public string GetNearMissLog(int[] index, string playerName, string[] shipNames)
    {
        string logText;

        string col = PositionManager.GetChartAlphabet(index[1]);
        int row = index[0] + 1;
        string shipName = "";
        for (int i = 0; i < shipNames.Length; i++)
        {
            if (shipNames[i] != NameDefinition.NOTSET)
            {
                if (shipName == "")
                {
                    shipName = shipNames[i];
                }
                else
                {
#if UNITY_WEBGL
                    shipName += "\" and \"" + shipNames[i];
#else
                    shipName += "\" と \"" + shipNames[i];
#endif
                }                
            }
        }

#if UNITY_WEBGL
        logText = "  " + playerName + " : "
            + "Attacked \""
            + row.ToString("0") + col + "\", and splashed water by \""
            + shipName
            +"\"!";
#else
        logText = "  " + playerName + " : "
            + "\""
            + row.ToString("0") + col + "\" を攻撃し、\""
            + shipName
            +"\" の近くで水しぶきが上がりました ! ";
#endif

        return logText;
    }
    
    // 攻撃が当たらなかった場合のログを取得
    public string GetAttackLog(int[] index, string playerName)
    {
        string logText;

        string col = PositionManager.GetChartAlphabet(index[1]);
        int row = index[0] + 1;

#if UNITY_WEBGL
        logText = "  " + playerName + " : "
            + "Attacked \""
            + row.ToString("0") + col + "\"!";
#else
        logText = "  " + playerName + " : "
            + "\""
            + row.ToString("0") + col + "\" を攻撃しました ! ";
#endif

        return logText;
    }

    // 座標同士の比較
    private bool JudgeEqual(int[] index1, int[] index2)
    {
        bool res = false;
        if (index1[0] == index2[0] && index1[1] == index2[1])
        {
            res = true;
        }

        return res;
    }
}
