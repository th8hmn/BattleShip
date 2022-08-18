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

    // 選択ブロックを色付けするか判断する
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

    // 選択範囲を取得し、色付けする
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

                playingObj = hit.collider.gameObject;   // 選択中のブロックを取得
                ChangeColor(player, playingObj);        // 色を変更する
                //playingObj.GetComponent<Renderer>().material.color = Color.blue;

                if (preObj != null && preObj != playingObj) // ブロックのエリアから外れたら色を初期状態に戻す
                {
                    ChangeToInitColor(preObj, Color.white);
                }
            }
            
            if (hit.collider.tag == NameDefinition.SHIPSTAG)    // 移動対象の戦艦を取得
            {
                preSelectShipObj = selectShipObj;

                selectShipObj = hit.collider.gameObject;    // 選択中の戦艦を取得

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
            if (playingObj != null)     // ブロックの選択がない場合、初期状態に戻す
            {
                preObj = playingObj;
                playingObj = null;

                ChangeToInitColor(preObj, Color.white);
            }
            if (selectShipObj != null)      // 移動対象の選択がない場合、初期状態に戻す
            {
                preSelectShipObj = selectShipObj;
                selectShipObj = null;

                ChangeToInitColor(preSelectShipObj, Color.white);
            }
        }
    }

    // 攻撃・移動の処理
    public bool[] AttackOrMove(GamePlayer player)
    {
        bool[] result = new bool[3];
        bool attacked = false;
        bool moved = false;
        bool isArea = true;
        if (GetPlayerObjExist())
        {
            if (player.attackFlag)  // 攻撃フラグが立っている場合
            {
                if (Array.IndexOf(player.attackAreaBlockNameList, playingObj.name) != -1)   // 選択が攻撃範囲に含まれる場合
                {
                    player.attackObjName = playingObj.name;
                    player.myturnFlag = false;
                    player.turnFinishFlag = true;
                    player.attackFlag = false;

                    attacked = true;    // 攻撃したことを設定
                    moved = false;
                    isArea = false;     // 未使用
                    player.moveToObjName = NameDefinition.NOTSET;
                }
            }
            else if (player.moveFlag)   // 移動フラグが立っている場合
            {
                if (Array.IndexOf(player.moveAreaBlockNameList, playingObj.name) != -1)     // 選択が移動範囲に含まれる場合
                {
                    player.moveToObjName = playingObj.name;
                    player.myturnFlag = false;
                    player.turnFinishFlag = true;
                    player.moveFlag = false;

                    attacked = false;
                    moved = true;   // 移動したことを設定
                    isArea = false; // 未使用
                }
            }
        }
        result[0] = attacked;
        result[1] = moved;
        result[2] = isArea; // 未使用

        return result;
    }

    // 移動対象の戦艦を取得
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

    // 選択中のブロックの色を変更する
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

    // 未使用
    private void ChangeBackGroundColor(GameObject gameObject)
    {
        if (isOnShip)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    // 選択状態でない場合、ブロックの色を初期状態に戻す
    public void UnSelect()
    {
        if (playingObj != null)
        {
            preObj = playingObj;
            playingObj = null;

            ChangeToInitColor(preObj, Color.white);
        }
    }

    // 選択状態でない場合、ブロックの色を初期状態に戻す
    public void UnSelect2()
    {
        if (preObj != null)
        {
            playingObj = null;

            ChangeToInitColor(preObj, Color.white);
        }
    }

    // ブロックを選択している場合、Trueを返す
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

    // GetPlayExist()とセットで呼び出す
    public int[] GetSelectPosition()
    {
        int[] pos = new int[2];
        pos = PositionManager.GetAddressFromGameObjectName(playingObj.name);    // 選択中のブロックの位置を取得

        return pos;
    }

    // 未使用
    private void DeployBattleShips(GameObject gameObject)
    {
        GameObject g = Instantiate(battleshipObj, gameObject.transform.position, Quaternion.identity);
        
    }

    // 未使用
    public void ResetColorOfSelectedShipBlock()
    {
        ChangeToInitColor(preBlockObj, Color.white);
    }

    // ブロックの色を初期状態に戻す
    private void ChangeToInitColor(GameObject gameObject, Color color)
    {
        gameObject.GetComponent<Renderer>().material.color = color;
    }

    // 選択中の戦艦がある場合、Trueを返す
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
