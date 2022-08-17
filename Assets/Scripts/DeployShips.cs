using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployShips : MonoBehaviour
{
    public GameObject battleshipObj;
    public GameObject destroyerObj;
    public GameObject submarineObj;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ��͂�z�u
    public void DeployBattleShip(GamePlayer player, Select select)
    {
        player.battleshipIdx = select.GetSelectPosition();
        Vector3 battleshipPos = PositionManager.GetPositionFromIndex(player.battleshipIdx);
        GameObject g = Instantiate(battleshipObj, battleshipPos, Quaternion.identity);
        g.name = NameDefinition.BATTLESHIP;
        select.UnSelect2();
        player.selectPhase = 2;
    }

    // �쒀�͂�z�u
    public void DeployDestroyer(GamePlayer player, Select select)
    {
        string selectName = PositionManager.GetBlockNameFromIndex(select.GetSelectPosition());
        string battleshipBlockName = PositionManager.GetBlockNameFromIndex(player.battleshipIdx);
        if (selectName != battleshipBlockName)
        {
            player.destroyerIdx = select.GetSelectPosition();
            Vector3 destroyerPos = PositionManager.GetPositionFromIndex(player.destroyerIdx);
            GameObject g = Instantiate(destroyerObj, destroyerPos, Quaternion.identity);
            g.name = NameDefinition.DESTROYER;
            select.UnSelect2();
            player.selectPhase = 3;
        }
    }

    // �����͂�z�u
    public void DeploySubmarine(GamePlayer player, Select select)
    {
        string selectName = PositionManager.GetBlockNameFromIndex(select.GetSelectPosition());
        string battleshipBlockName = PositionManager.GetBlockNameFromIndex(player.battleshipIdx);
        string destroyerBlockName = PositionManager.GetBlockNameFromIndex(player.destroyerIdx);
        if (selectName != battleshipBlockName && selectName != destroyerBlockName)
        {
            player.submarineIdx = select.GetSelectPosition();
            Vector3 submarinePos = PositionManager.GetPositionFromIndex(player.submarineIdx);
            GameObject g = Instantiate(submarineObj, submarinePos, Quaternion.identity);
            g.name = NameDefinition.SUBMARINE;
            select.UnSelect2();
            player.selectPhase = 4;
        }
    }

    // ��͂��ړ�
    public string[] MoveShip(GamePlayer player)
    {
        GameObject ship = GameObject.Find(player.selectShipName);
        string[] shipDataList = new string[3];

        shipDataList[0] = player.selectShipName;

        if (player.selectShipName == NameDefinition.BATTLESHIP)
        {
            // �ړ��O�u���b�N
            shipDataList[1] = PositionManager.GetBlockNameFromIndex(player.battleshipIdx);

            // �ړ�����
            player.battleshipIdx = PositionManager.GetAddressFromGameObjectName(player.moveToObjName);
            ship.transform.position = PositionManager.GetPositionFromIndex(player.battleshipIdx);

            // �ړ���u���b�N
            shipDataList[2] = PositionManager.GetBlockNameFromIndex(player.battleshipIdx);
        }
        else if (player.selectShipName == NameDefinition.DESTROYER)
        {
            // �ړ��O�u���b�N
            shipDataList[1] = PositionManager.GetBlockNameFromIndex(player.destroyerIdx);

            // �ړ�����
            player.destroyerIdx = PositionManager.GetAddressFromGameObjectName(player.moveToObjName);
            ship.transform.position = PositionManager.GetPositionFromIndex(player.destroyerIdx);

            // �ړ���u���b�N
            shipDataList[2] = PositionManager.GetBlockNameFromIndex(player.destroyerIdx);
        }
        else if (player.selectShipName == NameDefinition.SUBMARINE)
        {
            // �ړ��O�u���b�N
            shipDataList[1] = PositionManager.GetBlockNameFromIndex(player.submarineIdx);

            // �ړ�����
            player.submarineIdx = PositionManager.GetAddressFromGameObjectName(player.moveToObjName);
            ship.transform.position = PositionManager.GetPositionFromIndex(player.submarineIdx);

            // �ړ���u���b�N
            shipDataList[2] = PositionManager.GetBlockNameFromIndex(player.submarineIdx);
        }
        player.selectShipName = NameDefinition.NOTSET;

        return shipDataList;
    }

    public void SetHPDisplay()
    {

    }

    // �ړ��̍������擾
    public int[] GetMoveDifference(int difference)
    {
        int[] res = new int[2];
        if (difference > 0)
        {
            res[0] = 1;
        }
        else if (difference < 0)
        {
            res[0] = -1;
        }
        else if (difference == 0)
        {
            res[0] = 0;
        }
        res[1] = Mathf.Abs(difference);

        return res;
    }

    // �S�v���C���[���z�u�������������f����
    public bool JudgeAllPlayerDeployComp(GameObject[] players)
    {
        bool playersReady = false;

        for (int j = 0; j < players.Length; j++)
        {
            GameObject playerObj = players[j];
            GamePlayer player = playerObj.GetComponent<GamePlayer>();
            if (player.id <= 2)
            {
                if (!player.readyForBattle)
                {
                    playersReady = false;
                    break;
                }
                else
                {
                    playersReady = true;
                }
            }
        }

        return playersReady;
    }
}
