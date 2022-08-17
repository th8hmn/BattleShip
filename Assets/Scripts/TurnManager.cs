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

    // ��U�����߂�
    public void DecideFirstPlayer(GameObject[] players, GamePlayer player, bool debugFlag)
    {
        if (player.isMine && player.id == 1 && !player.isSetup) // id == 1�̏ꍇ�̂݁A��������
        {
            if (debugFlag)
            {
                player.rand = Random.Range(1, 2);//�f�o�b�O�p��2��ݒ�i���ۂ�3�j
            }
            else
            {
                player.rand = Random.Range(1, 3);//�f�o�b�O�p��2��ݒ�i���ۂ�3�j
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

    // �^�[�����
    public void ChangeTurn(GameObject[] players, GamePlayer player)
    {
        if (player.myturnPlayerID != player.id)
        {
            // ����^�[���I��
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

    // �^�[���I�����̏���
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
