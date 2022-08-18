using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Select : MonoBehaviour
{
    private Vector3 pos;
    private Vector3 mousePos;

    private GameObject playingObj;
    private GameObject preObj;

    private GameObject selectShipObj;
    private GameObject preSelectShipObj;

    public GameObject battleshipObj;

    private bool isOnShip;
    private GameObject preBlockObj;

    // Start is called before the first frame update
    void Start()
    {
        isOnShip = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // �I���u���b�N��F�t�����邩���f����
    public  bool SelectJudge(GamePlayer player, bool selectedShip)
    {
        if (player.id > 2)
        {
            return false;
        }
        else if (player.selectPhase <= 3 )
        {
            return true;
        }
        else if (player.myturnFlag && (player.attackFlag || player.moveFlag || !selectedShip))
        {
            return true;
        }
        else
        {
            UnSelect();
            return false;
        }
    }

    // �I��͈͂��擾���A�F�t������
    public void SelectArea(GamePlayer player)
    {
        mousePos = Input.mousePosition;
        mousePos.z = 10f;
        pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.tag == NameDefinition.BLOCKTAG)
            {
                preObj = playingObj;

                playingObj = hit.collider.gameObject;   // �I�𒆂̃u���b�N���擾
                ChangeColor(player, playingObj);        // �F��ύX����
                //playingObj.GetComponent<Renderer>().material.color = Color.blue;

                if (preObj != null && preObj != playingObj) // �u���b�N�̃G���A����O�ꂽ��F��������Ԃɖ߂�
                {
                    ChangeToInitColor(preObj, Color.white);
                }
            }
            
            if (hit.collider.tag == NameDefinition.SHIPSTAG)    // �ړ��Ώۂ̐�͂��擾
            {
                preSelectShipObj = selectShipObj;

                selectShipObj = hit.collider.gameObject;    // �I�𒆂̐�͂��擾

                Vector3 pos = selectShipObj.transform.position;
                int x_idx = (int)pos.x + 2;
                int y_idx = (int)pos.y + 2;
                string blockName = NameDefinition.BLOCK + x_idx.ToString("0") + y_idx.ToString("0");
                GameObject blockObj = GameObject.Find(blockName);

                isOnShip = true;

                //ChangeBackGroundColor(blockObj);

                preBlockObj = blockObj;
            }
            else
            {
                selectShipObj = null;
            }
        }
        else
        {
            if (playingObj != null)     // �u���b�N�̑I�����Ȃ��ꍇ�A������Ԃɖ߂�
            {
                preObj = playingObj;
                playingObj = null;

                ChangeToInitColor(preObj, Color.white);
            }
            if (selectShipObj != null)      // �ړ��Ώۂ̑I�����Ȃ��ꍇ�A������Ԃɖ߂�
            {
                preSelectShipObj = selectShipObj;
                selectShipObj = null;

                ChangeToInitColor(preSelectShipObj, Color.white);
            }
        }
    }

    // �U���E�ړ��̏���
    public bool[] AttackOrMove(GamePlayer player)
    {
        bool[] result = new bool[3];
        bool attacked = false;
        bool moved = false;
        bool isArea = true;
        if (GetPlayerObjExist())
        {
            if (player.attackFlag)  // �U���t���O�������Ă���ꍇ
            {
                if (Array.IndexOf(player.attackAreaBlockNameList, playingObj.name) != -1)   // �I�����U���͈͂Ɋ܂܂��ꍇ
                {
                    player.attackObjName = playingObj.name;
                    player.myturnFlag = false;
                    player.turnFinishFlag = true;
                    player.attackFlag = false;

                    attacked = true;    // �U���������Ƃ�ݒ�
                    moved = false;
                    isArea = false;     // ���g�p
                    player.moveToObjName = NameDefinition.NOTSET;
                }
            }
            else if (player.moveFlag)   // �ړ��t���O�������Ă���ꍇ
            {
                if (Array.IndexOf(player.moveAreaBlockNameList, playingObj.name) != -1)     // �I�����ړ��͈͂Ɋ܂܂��ꍇ
                {
                    player.moveToObjName = playingObj.name;
                    player.myturnFlag = false;
                    player.turnFinishFlag = true;
                    player.moveFlag = false;

                    attacked = false;
                    moved = true;   // �ړ��������Ƃ�ݒ�
                    isArea = false; // ���g�p
                }
            }
        }
        result[0] = attacked;
        result[1] = moved;
        result[2] = isArea; // ���g�p

        return result;
    }

    // �ړ��Ώۂ̐�͂��擾
    public bool SelectShip(GamePlayer player)
    {
        bool selected = false;
        if (GetSelectShipObjExist())
        {
            player.selectShipName = selectShipObj.name;
            selectShipObj = null;
            selected = true;
        }

        return selected;
    }

    // �I�𒆂̃u���b�N�̐F��ύX����
    private void ChangeColor(GamePlayer player,GameObject playingObj)
    {
        if (player.selectPhase <= 3)
        {
            string selected1 = PositionManager.GetBlockNameFromIndex(player.battleshipIdx);
            string selected2 = PositionManager.GetBlockNameFromIndex(player.destroyerIdx);
            if (playingObj.name != selected1 && playingObj.name != selected2)
            {
                playingObj.GetComponent<Renderer>().material.color = Color.blue;
            }
        }
        else if (player.attackFlag)
        {
            if (Array.IndexOf(player.attackAreaBlockNameList, playingObj.name) != -1)
            {
                playingObj.GetComponent<Renderer>().material.color = Color.blue;
            }
        }
        else if (player.moveFlag)
        {
            if (Array.IndexOf(player.moveAreaBlockNameList, playingObj.name) != -1)
            {
                playingObj.GetComponent<Renderer>().material.color = Color.blue;
            }
        }
    }

    // ���g�p
    private void ChangeBackGroundColor(GameObject gameObject)
    {
        if (isOnShip)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    // �I����ԂłȂ��ꍇ�A�u���b�N�̐F��������Ԃɖ߂�
    public void UnSelect()
    {
        if (playingObj != null)
        {
            preObj = playingObj;
            playingObj = null;

            ChangeToInitColor(preObj, Color.white);
        }
    }

    // �I����ԂłȂ��ꍇ�A�u���b�N�̐F��������Ԃɖ߂�
    public void UnSelect2()
    {
        if (preObj != null)
        {
            playingObj = null;

            ChangeToInitColor(preObj, Color.white);
        }
    }

    // �u���b�N��I�����Ă���ꍇ�ATrue��Ԃ�
    public bool GetPlayerObjExist()
    {
        bool exist;

        if (playingObj != null)
        {
            exist = true;
        }
        else
        {
            exist = false;
        }
        return exist;
    }

    // GetPlayExist()�ƃZ�b�g�ŌĂяo��
    public int[] GetSelectPosition()
    {
        int[] pos = new int[2];
        pos = PositionManager.GetAddressFromGameObjectName(playingObj.name);    // �I�𒆂̃u���b�N�̈ʒu���擾

        return pos;
    }

    // ���g�p
    private void DeployBattleShips(GameObject gameObject)
    {
        GameObject g = Instantiate(battleshipObj, gameObject.transform.position, Quaternion.identity);
        
    }

    // ���g�p
    public void ResetColorOfSelectedShipBlock()
    {
        ChangeToInitColor(preBlockObj, Color.white);
    }

    // �u���b�N�̐F��������Ԃɖ߂�
    private void ChangeToInitColor(GameObject gameObject, Color color)
    {
        gameObject.GetComponent<Renderer>().material.color = color;
    }

    // �I�𒆂̐�͂�����ꍇ�ATrue��Ԃ�
    public bool GetSelectShipObjExist()
    {
        bool exist;

        if (selectShipObj != null)
        {
            exist = true;
        }
        else
        {
            exist = false;
        }
        return exist;
    }
}
