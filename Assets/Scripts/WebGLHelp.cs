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
        // webgl�̏ꍇ�A�����̃A�o�^�[��o�^
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

    // �^�[����㎞�̃f�[�^��ێ�
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

    // �^�[����㎞�̃f�[�^�ŏ㏑��
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
