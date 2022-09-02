using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public bool debugFlag;

    private bool instruct1ActiveFlag;
    private bool instruct2ActiveFlag;

    private bool backOpeFlag;
    private bool deployDoneFlag;

    private bool playersReady;

    private bool attacked;
    private bool moved;
    private bool selectedShip;
    private bool isArea;
    private bool isClear;
    private string destroyedShip;
    private bool _hitFlag;
    private string _attackObjName;

    private bool isFinish;

    private bool particleStopped;
    private bool particlePlaying;

    // 0: not selected, 1: attack, 2: move
    private int selectAction;

    public GameObject selectObj;
    private Select select;

    public GameObject deployshipsObj;
    private DeployShips deployShips;

    public GameObject frameManagerObj;
    private FrameManager frameManager;

    public GameObject attackOrMoveAreaObj;
    private AttackOrMoveArea attackOrMoveArea;

    public GameObject UIManagerObj;
    private UIManager uIManager;

    public GameObject turnManagerObj;
    private TurnManager turnManager;

    public GameObject effectManagerObj;
    private EffectManager effectManager;
    
    public GameObject gameEndObj;
    private GameEnd gameEnd;

    public GameObject webGLHelp;

    public GameObject roomSettingsObj;
    private RoomSettings roomSettings;

    // Start is called before the first frame update
    void Start()
    {
        instruct1ActiveFlag = false;
        instruct2ActiveFlag = false;

        backOpeFlag = false;
        deployDoneFlag = false;

        playersReady = false;

        attacked = false;
        moved = false;
        selectedShip = false;
        isArea = false;
        isClear = false;
        destroyedShip = NameDefinition.NOTSET;
        _hitFlag = false;
        _attackObjName = NameDefinition.NOTSET;

        isFinish = false;

        particleStopped = false;
        particlePlaying = false;

        selectAction = 0;

        select = selectObj.GetComponent<Select>();
        deployShips = deployshipsObj.GetComponent<DeployShips>();
        frameManager = frameManagerObj.GetComponent<FrameManager>();
        attackOrMoveArea = attackOrMoveAreaObj.GetComponent<AttackOrMoveArea>();
        uIManager = UIManagerObj.GetComponent<UIManager>();
        turnManager = turnManagerObj.GetComponent<TurnManager>();
        effectManager = effectManagerObj.GetComponent<EffectManager>();
        gameEnd = gameEndObj.GetComponent<GameEnd>();
        roomSettings = roomSettingsObj.GetComponent<RoomSettings>();
    }
    // Update is called once per frame
    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(NameDefinition.PLAYERTAG);
        for (int i = 0; i < players.Length; i++)
        {
            GameObject playerObj = players[i];
            GamePlayer player = playerObj.GetComponent<GamePlayer>();
            UnityEngine.Debug.Log(player.hideSubmarineOpt);

            // ���[���ݒ肷��
            roomSettings.SettingOptions(players, player);

            // ��U�����߂�
            turnManager.DecideFirstPlayer(players, player, debugFlag);

            // �^�[���̌��
            turnManager.ChangeTurn(players, player);
            
            // �S�v���C���[���z�u�̔z�u�����𒲂ׂ�
            if (!playersReady)
            {
                playersReady = deployShips.JudgeAllPlayerDeployComp(players);
            }

            // -------------------------------------
            // �e�v���[���[���Ƃɕ`��
            // -------------------------------------
            if (player.isMine)
            {
                particleStopped = effectManager.GetParticleSystemStatus();

                if (players.Length >= 2)
                {
                    // �J�[�\���𓖂ĂĂ���u���b�N�ɐF��t����
                    if (select.SelectJudge(player, selectedShip))
                    {
                        select.SelectArea(player);
                    }
                    else if (player.id > 2)     // 3�l�ڈȍ~�̓��O�ȊO�̕\��������
                    {
                        uIManager.GoActionPhase(player.id);
                    }

                    // �z�u�I���̑J��
                    if (player.selectPhase == 1)
                    {
                        // ��͔z�u
                        uIManager.GoSelectPhase1();
                    }
                    else if (player.selectPhase == 2)
                    {
                        // �쒀�͔z�u
                        uIManager.GoSelectPhase2();
                    }
                    else if (player.selectPhase == 3)
                    {
                        // �����͔z�u
                        uIManager.GoSelectPhase3();
                    }
                    else if (player.selectPhase == 4)
                    {
                        // �z�u�����{�^���\��
                        uIManager.GoSelectPhase4();
                    }
                }

                if (player.id > 2)
                {
                    uIManager.SetPlayerName(players, player.id);
                    uIManager.GoActionPhase(player.id);
                }

                // -------------------------------------
                // �Q�[���J�n
                // -------------------------------------
                // �z�u�����{�^�����\��
                if (player.readyForBattle)
                {
                    uIManager.GoActionPhase(player.id);
                    uIManager.DisplayHP(player);
#if (UNITY_WEBGL)
                    webGLHelp.SetActive(true);
#endif
                }

                // �U���q�b�g���ɃG�t�F�N�g�\��
                if (player.myturnFlag && _hitFlag)
                {
                    // �G�t�F�N�g�\���ʒu�ݒ�
                    int[] address = new int[2];
                    Vector3 pos;
                    address = PositionManager.GetAddressFromGameObjectName(_attackObjName);
                    pos = PositionManager.GetPositionFromIndex(address);

                    // �G�t�F�N�g�\��
                    uIManager.HideWaitText();
                    effectManager.PlayExplosionEffect(pos);
                    particlePlaying = true;     // �G�t�F�N�g�\�����t���O

                    // �j�󂷂�D���ݒ肳��Ă�����j�󂷂�
                    if (destroyedShip != NameDefinition.NOTSET)
                    {
                        attackOrMoveArea.DestroyShips(player, destroyedShip);
                        destroyedShip = NameDefinition.NOTSET;
                    }

                    _hitFlag = false;
                }

                // �U���ƈړ��̃{�^����\��/��\��
                if (player.finishGameFlag && !isFinish)  // �Q�[���I���̏ꍇ
                {
                    if (particlePlaying)    // �G�t�F�N�g���̏ꍇ
                    {
                        if (particleStopped)    // �G�t�F�N�g������������
                        {
                            string finishText = "You lose...";
                            uIManager.EndGame(finishText);
                            isFinish = true;
                            particlePlaying = false;
                        }
                    }
                }
                else                       // �Q�[���I���łȂ��ꍇ
                {
                    if (playersReady && player.readyForBattle && player.myturnFlag && !isFinish) // �����̃^�[���ɕ\��
                    {
                        if (particlePlaying)    // �G�t�F�N�g���̏ꍇ
                        {
                            if (particleStopped)    // �G�t�F�N�g������������
                            {
                                uIManager.StartTurn();
                                particlePlaying = false;
                            }
                        }
                        else
                        {
                            uIManager.StartTurn();
                        }

                    }
                    else if (player.readyForBattle && !player.myturnFlag)   // ����^�[���ɔ�\��
                    {
                        uIManager.FinishTurn(isFinish);
                    }
                }

                // -------------------------------------
                // ��������I�����ɏ��������s
                // -------------------------------------
                if (player.decision != 0)
                {
                    // �z�u�u���b�N���I�����ꂽ��A��͂�z�u
                    if (select.GetPlayerObjExist())
                    {
                        if (player.selectPhase == 1)
                        {
                            deployShips.DeployBattleShip(player, select);
                            player.decision = 0;
                        }
                        else if (player.selectPhase == 2)
                        {
                            deployShips.DeployDestroyer(player, select);
                            player.decision = 0;
                        }
                        else if (player.selectPhase == 3)
                        {
                            deployShips.DeploySubmarine(player, select);
                            player.decision = 0;
                        }
                    }

                    // �߂�{�^���������ꂽ�Ƃ��A�z�u�����߂�
                    if (backOpeFlag)
                    {
                        backOperation(player);
                        backOpeFlag = false;
                    }

                    // �z�u�����̃{�^���������ꂽ�Ƃ��Aplayer�̔z�u�����̃t���O�𗧂Ă�
                    if (deployDoneFlag)
                    {
                        player.readyForBattle = true;
                        uIManager.SetPlayerName(players, player.id);
                        deployDoneFlag = false;
                    }

                    // -------------------------------------
                    // �����̃^�[���̏���
                    // -------------------------------------
                    if (player.myturnFlag)
                    {
                        // �U���E�ړ��͈͂���шړ��֘A�̃t���O�̏�����
                        if (player.readyForBattle && !isClear)
                        {
                            attackOrMoveArea.InitAttackAreaIndex(player);
                            attackOrMoveArea.InitMoveAreaIndex(player);
                            player.selectShipName = NameDefinition.NOTSET;
                            player.moveFlag = false;
                            isClear = true;
                        }
                        // �U�����ړ���I��
                        if (player.readyForBattle && !isArea)
                        {
                            isArea = attackOrMoveArea.SetAttackOrMoveAreaIndex(player, player.battleshipIdx, player.destroyerIdx, player.submarineIdx);
                        }

                        // �A�N�V�����I����̏����i�t���O����E�g�̐F�ݒ�j
                        if (selectAction == 0)
                        {
                            frameManager.InitColor(player);
                        }
                        else if (selectAction == 1) // �U�����̏���
                        {
                            frameManager.InitColor(player);
                            frameManager.SetColorAttackArea(player);
                            player.attackFlag = true;
                            player.moveFlag = false;
                        }
                        else if (selectAction == 2) // �ړ����̏���
                        {
                            frameManager.InitColor(player);
                            player.attackFlag = false;
                            if (!selectedShip)
                            {
                                selectedShip = select.SelectShip(player);
                            }
                            else
                            {
                                frameManager.SetColorMoveArea(player);
                                player.moveFlag = true;
                            }
                        }

                        // �U�����ړ�
                        if (player.attackFlag || player.moveFlag)
                        {
                            bool[] actionResult = new bool[3];
                            //
                            // �^�[���I���Ɍ���������
                            //
                            actionResult = select.AttackOrMove(player);
                            attacked = actionResult[0]; // �U�������ꍇ
                            moved = actionResult[1];    // �ړ������ꍇ
                            isArea = actionResult[2];   // �I��͈͂𖢐ݒ�ɂ���
                        }
                    }
                    else
                    {
                        // ����^�[���́A�g�̐F��������Ԃɖ߂�
                        frameManager.InitColor(player);
                    }

                    // �U�����̏���
                    string[] shipsBlocks = new string[3];

                    // �G��͂̈ʒu�擾
                    for (int j = 0; j < players.Length; j++)
                    {
                        GameObject tmpPlayerObj = players[j];
                        GamePlayer tmpPlayer = tmpPlayerObj.GetComponent<GamePlayer>();
                        if (player.id == 1 && tmpPlayer.id == 2)
                        {
                            for (int k = 0; k < shipsBlocks.Length; k++)
                            {
                                shipsBlocks[0] = PositionManager.GetBlockNameFromIndex(tmpPlayer.battleshipIdx);
                                shipsBlocks[1] = PositionManager.GetBlockNameFromIndex(tmpPlayer.destroyerIdx);
                                shipsBlocks[2] = PositionManager.GetBlockNameFromIndex(tmpPlayer.submarineIdx);
                            }
                        }
                        else if (player.id == 2 && tmpPlayer.id == 1)
                        {
                            for (int k = 0; k < shipsBlocks.Length; k++)
                            {
                                shipsBlocks[0] = PositionManager.GetBlockNameFromIndex(tmpPlayer.battleshipIdx);
                                shipsBlocks[1] = PositionManager.GetBlockNameFromIndex(tmpPlayer.destroyerIdx);
                                shipsBlocks[2] = PositionManager.GetBlockNameFromIndex(tmpPlayer.submarineIdx);
                            }
                        }
                    }

                    // ----------------------------------------
                    // �U���Ώۂ�I�����̏���
                    // ----------------------------------------
                    if (attacked)
                    {
                        bool isHit = false;
                        bool isNearMiss = false;
                        int[] attackAreaIdx = PositionManager.GetAddressFromGameObjectName(player.attackObjName);
                        int[][] shipIdx = new int[3][];
                        for (int j = 0; j < shipsBlocks.Length; j++)
                        {
                            shipIdx[j] = PositionManager.GetAddressFromGameObjectName(shipsBlocks[j]);
                        }
                        
                        string[][] nearMissArea = new string[3][];
                        for (int j = 0; j < shipsBlocks.Length; j++)
                        {
                            nearMissArea[j] = attackOrMoveArea.GetAttackAreaIndex(shipIdx[j]);
                        }

                        // �U�������������Ƃ��̏���
                        for (int j = 0; j < shipsBlocks.Length; j++)
                        {
                            if (player.attackObjName == shipsBlocks[j])
                            {
                                isHit = true;

                                // HP����
                                player.opponentShipsHP[j]--;
                                player.hitFlag = true;

                                // ���O�X�V
                                string logText = attackOrMoveArea.GetHitLog(attackAreaIdx, player.playerName, j);
                                player.logText = GetLogText(player.logText, logText);
                                uIManager.DisplayLog(player.logText);
                                player.logUpdateFlag = true;

                                // �c�0�ɂȂ�����j�󂷂�D��ݒ肷��
                                if (player.opponentShipsHP[j] == 0)
                                {
                                    player.destroyedShipName = NameDefinition.SHIPLIST[j];
                                }
                                break;
                            }
                        }

                        // �U�������������Ƃ��ɃQ�[���I��������s��
                        if (player.hitFlag)
                        {
                            player.finishGameFlag = gameEnd.SetFinishGameFlag(player.opponentShipsHP);
                        }

                        // �U�����j�A�~�X���������̏���
                        if (!isHit)
                        {
                            string[] nearMissShips = new string[3];
                            // �j�A�~�X�Ώې�͂̎擾
                            for (int j = 0; j < shipsBlocks.Length; j++)
                            {
                                if (attackOrMoveArea.IsInList(nearMissArea[j], player.attackObjName))
                                {
                                    nearMissShips[j] = NameDefinition.SHIPLIST[j];
                                    isNearMiss = true;
                                }
                                else
                                {
                                    nearMissShips[j] = NameDefinition.NOTSET;
                                }
                            }
                            // ���O�X�V
                            if (isNearMiss)
                            {
                                string logText = attackOrMoveArea.GetNearMissLog(attackAreaIdx, player.playerName, nearMissShips);
                                player.logText = GetLogText(player.logText, logText);
                                uIManager.DisplayLog(player.logText);
                                player.logUpdateFlag = true;
                            }
                        }

                        // �U����������Ȃ������Ƃ��̃��O�X�V
                        if (!isHit! && !isNearMiss)
                        {
                            string logText = attackOrMoveArea.GetAttackLog(attackAreaIdx, player.playerName);
                            player.logText = GetLogText(player.logText, logText);
                            uIManager.DisplayLog(player.logText);
                            player.logUpdateFlag = true;
                        }
                        attackOrMoveArea.InitAttackAreaIndex(player);
                        attacked = false;
                        selectAction = 0;
                    }

                    // ----------------------------------------
                    // �ړ����̏���
                    // ----------------------------------------
                    if (moved)
                    {
                        if (player.moveToObjName != NameDefinition.NOTSET && player.selectShipName != NameDefinition.NOTSET)
                        {
                            string logText = attackOrMoveArea.GetMoveLog(deployShips, player);
                            player.logText = GetLogText(player.logText, logText);
                            uIManager.DisplayLog(player.logText);
                            player.logUpdateFlag = true;
                            attackOrMoveArea.InitMoveAreaIndex(player);
                            moved = false;
                            selectAction = 0;
                        }
                    }

                    // �^�[���I�����Ɏ��̃v���C���[��ݒ�
                    turnManager.EndTurn(player);

                    // �Q�[���I���̏ꍇ
                    if (player.finishGameFlag && !isFinish)
                    {
                        string finishText = "You win!!!";
                        uIManager.EndGame(finishText);
                        isFinish = true;
                    }

                    // �`��̂���0.2s�҂��Ă���A���������t���O������
                    StartCoroutine(DelayMethod(0.2f, player));
                }
            }
            else
            {
                // ���̍X�V
                for (int j = 0; j < players.Length; j++)
                {
                    GameObject tmpPlayerObj = players[j];
                    GamePlayer tmpPlayer = tmpPlayerObj.GetComponent<GamePlayer>();
                    if (tmpPlayer.id != player.id)
                    {
                        // �U�������������ꍇ�AHP�̍X�V
                        if (player.hitFlag && tmpPlayer.id <= 2)
                        {
                            _hitFlag = true;
                            _attackObjName = player.attackObjName;
                            for (int k = 0; k < player.shipsHP.Length; k++)
                            {
                                tmpPlayer.shipsHP[k] = player.opponentShipsHP[k];
                            }
                            destroyedShip = player.destroyedShipName;
                            tmpPlayer.finishGameFlag = player.finishGameFlag;
                            player.hitFlag = false;
                            player.destroyedShipName = NameDefinition.NOTSET;
                            player.attackObjName = NameDefinition.NOTSET;
                        }
                        // ���O�̍X�V
                        if (player.logUpdateFlag)
                        {
#if (!UNITY_WEBGL)
                            if (tmpPlayer.id > 2)
                            {
                                if (player.id == 1)
                                {
                                    tmpPlayer.logText = player.logText;
                                    uIManager.DisplayLog(tmpPlayer.logText);
                                }
                                else if (player.id == 2)
                                {
                                    tmpPlayer.logText2 = player.logText;
                                    uIManager.DisplayLog2(tmpPlayer.logText2);
                                }
                            }
                            else if (tmpPlayer.id <= 2)
                            {
                                tmpPlayer.logText2 = player.logText;
                                uIManager.DisplayLog2(tmpPlayer.logText2);
                            }
#else
                            tmpPlayer.logText = player.logText;
                            uIManager.DisplayLog(tmpPlayer.logText);
#endif
                            player.logUpdateFlag = false;
                        }
                    }                    
                }
            }
        }
    }

    void DelayMethod()
    {
        Debug.Log("Delay call");
    }

    IEnumerator DelayMethod(float delay, GamePlayer player)
    {
        //delay�b�҂�
        yield return new WaitForSeconds(delay);
        /*����*/
        if (debugFlag)
        {

        }
        else
        {
            player.decision = 0;
        }       
    }

    IEnumerator DelayMethod2(float delay, GamePlayer player)
    {
        //delay�b�҂�
        yield return new WaitForSeconds(delay);
        /*����*/
        select.AttackOrMove(player);
    }

    // ���O�����擾
    private string GetLogText(string baseLog, string logText)
    {
        string outLog;

        if (baseLog == "")
        {
            outLog = "\n" + logText + "\n";
        }
        else
        {
            outLog = baseLog + "\n" + logText + "\n";
        }

        return outLog;
    }

    // �߂�{�^�����������Ƃ��Ƀt���O�𗧂Ă�
    public void SetBackOpeFlag()
    {
        backOpeFlag = true;
    }

    // �߂�{�^���Ő�͂̔z�u����߂�
    public void backOperation(GamePlayer player)
    {
        if (player.selectPhase == 2)    // ��͔z�u�L�����Z��
        {
            GameObject gameObject = GameObject.Find(NameDefinition.BATTLESHIP);
            if (gameObject != null)
            {
                Destroy(gameObject);
                for (int i = 0; i < player.battleshipIdx.Length; i++)
                {
                    player.battleshipIdx[i] = PositionManager.INITPOSITION;
                }
                player.selectPhase = 1;
            }
        }
        else if (player.selectPhase == 3)   // �쒀�͔z�u�L�����Z��
        {
            GameObject gameObject = GameObject.Find(NameDefinition.DESTROYER);
            if (gameObject != null)
            {
                Destroy(gameObject);
                for (int i = 0; i < player.destroyerIdx.Length; i++)
                {
                    player.destroyerIdx[i] = PositionManager.INITPOSITION;
                }
                player.selectPhase = 2;
            }
        }
        else if (player.selectPhase == 4)   // �����͔z�u�L�����Z��
        {
            GameObject gameObject = GameObject.Find(NameDefinition.SUBMARINE);
            if (gameObject != null)
            {
                Destroy(gameObject);
                for (int i = 0; i < player.submarineIdx.Length; i++)
                {
                    player.submarineIdx[i] = PositionManager.INITPOSITION;
                }
                player.selectPhase = 3;
            }
        }
    }

    // �z�u�����{�^�����������Ƃ��ɔz�u�����t���O�𗧂Ă�
    public void DecideDeploy()
    {
        deployDoneFlag = true;
    }

    // �U���{�^�����������ꍇ�̏���
    public void SelectAttackAction()
    {
        selectAction = 1;   // �U��
        selectedShip = false;   // �ړ��Ώې�͑I����ԃL�����Z��
        isArea = false;     // ���g�p
        isClear = false;    // �������
    }

    // �ړ��{�^�����������ꍇ�̏���
    public void SelectMoveAction()
    {
        selectAction = 2;   // �ړ�
        selectedShip = false;   // �ړ��Ώې�͑I����ԃL�����Z��
        isArea = false;     // ���g�p
        isClear = false;    // �������
    }
}
