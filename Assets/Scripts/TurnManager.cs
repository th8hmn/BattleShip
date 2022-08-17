using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 先攻を決める
    public void DecideFirstPlayer(GameObject[] players, GamePlayer player, bool debugFlag)
    {
        if (player.isMine && player.id == 1 && !player.isSetup) // id == 1の場合のみ、乱数生成
        {
            if (debugFlag)
            {
                player.rand = Random.Range(1, 2);//デバッグ用に2を設定（実際は3）
            }
            else
            {
                player.rand = Random.Range(1, 3);//デバッグ用に2を設定（実際は3）
            }
        }
        if (!player.isSetup && player.rand != 0)
        {
            if (player.id == player.rand)
            {
                player.myturnFlag = true;
                player.isSetup = true;
            }
            else
            {
                player.myturnFlag = false;
                player.isSetup = true;
            }
        }
        else if (player.rand == 0)
        {
            for (int j = 0; j < players.Length; j++)
            {
                GameObject tmpPlayerObj = players[j];
                GamePlayer tmpPlayer = tmpPlayerObj.GetComponent<GamePlayer>();
                if (tmpPlayer.rand != 0)
                {
                    player.rand = tmpPlayer.rand;
                }
            }
        }
    }

    // ターン交代
    public void ChangeTurn(GameObject[] players, GamePlayer player)
    {
        if (player.myturnPlayerID != player.id)
        {
            // 相手ターン終了
            if (player.turnFinishFlag)
            {
                for (int j = 0; j < players.Length; j++)
                {
                    GameObject tmpPlayerObj = players[j];
                    GamePlayer tmpPlayer = tmpPlayerObj.GetComponent<GamePlayer>();
                    if (tmpPlayer.id == player.myturnPlayerID)
                    {
                        tmpPlayer.myturnPlayerID = player.myturnPlayerID;
                        tmpPlayer.myturnFlag = true;
                        break;
                    }
                }
            }
        }
    }

    // ターン終了時の処理
    public void EndTurn(GamePlayer player)
    {
        if (player.turnFinishFlag)
        {
            if (player.id == 1)
            {
                player.myturnPlayerID = 2;
            }
            else if (player.id == 2)
            {
                player.myturnPlayerID = 1;
            }
        }
    }
}
