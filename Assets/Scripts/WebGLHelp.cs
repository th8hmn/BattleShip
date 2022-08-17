using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGLHelp : MonoBehaviour
{
    private GamePlayer player;

    private bool turnFinishFlag;
    private bool logUpdateFlag;
    private bool hitFlag;
    private string destroyedShipName;
    private bool turnChange;

    // Start is called before the first frame update
    void Start()
    {
        turnFinishFlag = false;
        logUpdateFlag = false;
        hitFlag = false;
        destroyedShipName = NameDefinition.NOTSET;
        turnChange = false;
    }

    // Update is called once per frame
    void Update()
    {
        // webglの場合、自分のアバターを登録
#if (UNITY_WEBGL)
        GameObject[] players = GameObject.FindGameObjectsWithTag(NameDefinition.PLAYERTAG);
        for (int i = 0; i < players.Length; i++)
        {
            GameObject playerObj = players[i];
            GamePlayer tmpPlayer = playerObj.GetComponent<GamePlayer>();
            if (tmpPlayer.isMine)
            {
                player = tmpPlayer;
            }
        }
#endif
    }

    // ターン交代時のデータを保持
    public void SetPlayerData(bool turnFinish, bool logUpdate, bool hit, string destroyedShip, bool turn)
    {
        if (player != null)
        {
            turnFinishFlag = turnFinish;
            logUpdateFlag = logUpdate;
            hitFlag = hit;
            destroyedShipName = destroyedShip;
            turnChange = turn;
        }
    }

    // ターン交代時のデータで上書き
    public void Reset()
    {
        if (player != null)
        {
            player.turnFinishFlag = turnFinishFlag;
            player.logUpdateFlag = logUpdateFlag;
            player.hitFlag = hitFlag;
            player.destroyedShipName = destroyedShipName;
            player.turnChange = turnChange;
        }
    }
}
