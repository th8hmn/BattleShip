using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GamePlayer : MonoBehaviourPunCallbacks, IPunObservable
{
    public int rand;
    public bool isSetup;
    public int hideSubmarineOpt;
    public int noteOpt;

    public int selectPhase;
    public bool readyForBattle;

    public bool attackFlag;
    public string attackObjName;
    public bool moveFlag;
    public string moveToObjName;
    public string selectShipName;
    public string destroyedShipName;

    public bool myturnFlag;
    public bool turnFinishFlag;
    public bool finishGameFlag;
    public int myturnPlayerID;

    public int[] battleshipIdx = new int[2];
    public int[] destroyerIdx = new int[2];
    public int[] submarineIdx = new int[2];
    public int[] shipsHP = new int[3];
    public int[] opponentShipsHP = new int[3];
    public bool hitFlag;

    public string[] attackAreaBlockNameList = new string[27];
    public string[] moveAreaBlockNameList = new string[9];

    public int decision;
    private int sendDecision;
    public bool sendData;

    public string logText;
    public string logText2;
    public bool logUpdateFlag;

    public int id;
    public string playerName;
    private PhotonView photonView;

    public bool turnChange;
    private bool preTurnChange;
    private bool downOfTurnChange;
    private int preMyturnPlayerID;

    public bool isMine;

#if (UNITY_WEBGL)
    private WebGLHelp webglhelp;
#endif

    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーのIDを取得する。
        photonView = this.GetComponent<PhotonView>();
        id = photonView.OwnerActorNr;
        //playerName = NameDefinition.NOTSET;
        sendData = false;

        rand = 0;
        isSetup = false;
        hideSubmarineOpt = 0;
        noteOpt = 0;

        selectPhase = 1;
        readyForBattle = false;
        downOfTurnChange = false;

        attackFlag = false;
        attackObjName = NameDefinition.NOTSET;
        moveFlag = false;
        moveToObjName = NameDefinition.NOTSET;
        selectShipName = NameDefinition.NOTSET;
        destroyedShipName = NameDefinition.NOTSET;

        myturnFlag = false;
        turnFinishFlag = false;
        finishGameFlag = false;

        myturnPlayerID = 0;
        preMyturnPlayerID = 0;
        turnChange = false;

        for (int i = 0; i < battleshipIdx.Length; i++)
        {
            battleshipIdx[i] = PositionManager.INITPOSITION;
        }
        for (int i = 0; i < destroyerIdx.Length; i++)
        {
            destroyerIdx[i] = PositionManager.INITPOSITION;
        }
        for (int i = 0; i < submarineIdx.Length; i++)
        {
            submarineIdx[i] = PositionManager.INITPOSITION;
        }
        for (int i = 0; i < attackAreaBlockNameList.Length; i++)
        {
            attackAreaBlockNameList[i] = PositionManager.NOTSET;
        }
        for (int i = 0; i < moveAreaBlockNameList.Length; i++)
        {
            moveAreaBlockNameList[i] = PositionManager.NOTSET;
        }

        for (int i = 0; i < shipsHP.Length; i++)
        {
            shipsHP[i] = 3 - i;
        }
        for (int i = 0; i < shipsHP.Length; i++)
        {
            opponentShipsHP[i] = 3 - i;
        }

        hitFlag = false;

        logText = "";
        logText2 = "";
        logUpdateFlag = false;

#if (UNITY_WEBGL)
    webglhelp = GameObject.Find("WebGLHelpManager").GetComponent<WebGLHelp>();
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            isMine = true;
            if (Input.GetMouseButtonDown(0))
            {
                if (sendDecision == 0)
                {
                    decision = 1;
                    sendData = true;
                }
            }
            if (!turnChange && preMyturnPlayerID != myturnPlayerID)
            {
                turnChange = true;
            }
            preMyturnPlayerID = myturnPlayerID;

            if (!downOfTurnChange && !turnChange && preTurnChange)
            {
                downOfTurnChange = true;
            }
            preTurnChange = turnChange;
        }
        else
        {
            isMine = false;
        }

        //何かしらをクリックした場合、送信用に保持しておく
        if (decision != 0)
        {
            sendDecision = decision;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // オーナーの場合
        if (stream.IsWriting)
        {
            stream.SendNext(this.sendDecision);
            stream.SendNext(this.playerName);
            stream.SendNext(this.hideSubmarineOpt);
            stream.SendNext(this.noteOpt);
            stream.SendNext(this.myturnFlag);
            stream.SendNext(this.rand);
            stream.SendNext(this.myturnPlayerID);
            stream.SendNext(this.turnFinishFlag);
            stream.SendNext(this.turnChange);
            stream.SendNext(this.readyForBattle);
            stream.SendNext(this.opponentShipsHP);
            stream.SendNext(this.attackObjName);
            stream.SendNext(this.hitFlag);
            stream.SendNext(this.battleshipIdx);
            stream.SendNext(this.destroyerIdx);
            stream.SendNext(this.submarineIdx);
            stream.SendNext(this.logText);
            stream.SendNext(this.logText2);
            stream.SendNext(this.logUpdateFlag);
            stream.SendNext(this.destroyedShipName);
            stream.SendNext(this.finishGameFlag);

            if (turnChange)
            {
#if (UNITY_WEBGL)
                webglhelp.SetPlayerData(turnFinishFlag, logUpdateFlag, hitFlag, destroyedShipName, turnChange);
#endif
                turnFinishFlag = false;
                logUpdateFlag = false;
                hitFlag = false;
                destroyedShipName = NameDefinition.NOTSET;
                turnChange = false;
            }
            sendDecision = 0;
        }
        // オーナー以外の場合
        else
        {
            this.decision = (int)stream.ReceiveNext();
            this.playerName = (string)stream.ReceiveNext();
            this.hideSubmarineOpt = (int)stream.ReceiveNext();
            this.noteOpt = (int)stream.ReceiveNext();
            this.myturnFlag = (bool)stream.ReceiveNext();
            this.rand = (int)stream.ReceiveNext();
            this.myturnPlayerID = (int)stream.ReceiveNext();
            this.turnFinishFlag = (bool)stream.ReceiveNext();
            this.turnChange = (bool)stream.ReceiveNext();
            this.readyForBattle = (bool)stream.ReceiveNext();
            this.opponentShipsHP = (int[])stream.ReceiveNext();
            this.attackObjName = (string)stream.ReceiveNext();
            this.hitFlag = (bool)stream.ReceiveNext();
            this.battleshipIdx = (int[])stream.ReceiveNext();
            this.destroyerIdx = (int[])stream.ReceiveNext();
            this.submarineIdx = (int[])stream.ReceiveNext();
            this.logText = (string)stream.ReceiveNext();
            this.logText2 = (string)stream.ReceiveNext();
            this.logUpdateFlag = (bool)stream.ReceiveNext();
            this.destroyedShipName = (string)stream.ReceiveNext();
            this.finishGameFlag = (bool)stream.ReceiveNext();
        }
    }
}
