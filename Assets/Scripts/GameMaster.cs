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
    }
    // Update is called once per frame
    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(NameDefinition.PLAYERTAG);
        for (int i = 0; i < players.Length; i++)
        {
            GameObject playerObj = players[i];
            GamePlayer player = playerObj.GetComponent<GamePlayer>();

            // 先攻を決める
            turnManager.DecideFirstPlayer(players, player, debugFlag);

            // ターンの交代
            turnManager.ChangeTurn(players, player);
            
            // 全プレイヤーが配置の配置完了を調べる
            if (!playersReady)
            {
                playersReady = deployShips.JudgeAllPlayerDeployComp(players);
            }

            // -------------------------------------
            // 各プレーヤーごとに描画
            // -------------------------------------
            if (player.isMine)
            {
                particleStopped = effectManager.GetParticleSystemStatus();

                if (players.Length >= 2)
                {
                    // カーソルを当てているブロックに色を付ける
                    if (select.SelectJudge(player, selectedShip))
                    {
                        select.SelectArea(player);
                    }

                    // 配置選択の遷移
                    if (player.selectPhase == 1)
                    {
                        // 戦艦配置
                        uIManager.GoSelectPhase1();
                    }
                    else if (player.selectPhase == 2)
                    {
                        // 駆逐艦配置
                        uIManager.GoSelectPhase2();
                    }
                    else if (player.selectPhase == 3)
                    {
                        // 潜水艦配置
                        uIManager.GoSelectPhase3();
                    }
                    else if (player.selectPhase == 4)
                    {
                        // 配置完了ボタン表示
                        uIManager.GoSelectPhase4();
                    }
                }

                // -------------------------------------
                // ゲーム開始
                // -------------------------------------
                // 配置完了ボタンを非表示
                if (player.readyForBattle)
                {
                    uIManager.GoActionPhase();
                    uIManager.DisplayHP(player);
#if (UNITY_WEBGL)
                    webGLHelp.SetActive(true);
#endif
                }

                // 攻撃ヒット時にエフェクト表示
                if (player.myturnFlag && player.hitFlag)
                {
                    // エフェクト表示位置設定
                    int[] address = new int[2];
                    Vector3 pos;
                    address = PositionManager.GetAddressFromGameObjectName(player.attackObjName);
                    pos = PositionManager.GetPositionFromIndex(address);

                    // エフェクト表示
                    effectManager.PlayExplosionEffect(pos);
                    particlePlaying = true;     // エフェクト表示中フラグ

                    // 破壊する船が設定されていたら破壊する
                    if (player.destroyedShipName != NameDefinition.NOTSET)
                    {
                        attackOrMoveArea.DestroyShips(player, player.destroyedShipName);
                    }

                    player.hitFlag = false;
                }

                // 攻撃と移動のボタンを表示/非表示
                if (player.finishGameFlag && !isFinish)  // ゲーム終了の場合
                {
                    string finishText = "You lose...";
                    uIManager.EndGame(finishText);
                    isFinish = true;
                }
                else                       // ゲーム終了でない場合
                {
                    if (playersReady && player.readyForBattle && player.myturnFlag && !isFinish) // 自分のターンに表示
                    {
                        if (particlePlaying)    // エフェクト中の場合
                        {
                            if (particleStopped)    // エフェクトが完了したら
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
                    else if (player.readyForBattle && !player.myturnFlag)   // 相手ターンに非表示
                    {
                        uIManager.FinishTurn();
                    }
                }

                // -------------------------------------
                // 何かしら選択時に処理を実行
                // -------------------------------------
                if (player.decision != 0)
                {
                    // 配置ブロックが選択されたら、戦艦を配置
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

                    // 戻るボタンが押されたとき、配置操作を戻す
                    if (backOpeFlag)
                    {
                        backOperation(player);
                        backOpeFlag = false;
                    }

                    // 配置完了のボタンが押されたとき、playerの配置完了のフラグを立てる
                    if (deployDoneFlag)
                    {
                        player.readyForBattle = true;
                        uIManager.SetPlayerName(players, player.id);
                        deployDoneFlag = false;
                    }

                    // -------------------------------------
                    // 自分のターンの処理
                    // -------------------------------------
                    if (player.myturnFlag)
                    {
                        // 攻撃・移動範囲および移動関連のフラグの初期化
                        if (player.readyForBattle && !isClear)
                        {
                            attackOrMoveArea.InitAttackAreaIndex(player);
                            attackOrMoveArea.InitMoveAreaIndex(player);
                            player.selectShipName = NameDefinition.NOTSET;
                            player.moveFlag = false;
                            isClear = true;
                        }
                        // 攻撃か移動を選択
                        if (player.readyForBattle && !isArea)
                        {
                            isArea = attackOrMoveArea.SetAttackOrMoveAreaIndex(player, player.battleshipIdx, player.destroyerIdx, player.submarineIdx);
                        }

                        // アクション選択後の処理（フラグ操作・枠の色設定）
                        if (selectAction == 0)
                        {
                            frameManager.InitColor(player);
                        }
                        else if (selectAction == 1) // 攻撃時の処理
                        {
                            frameManager.InitColor(player);
                            frameManager.SetColorAttackArea(player);
                            player.attackFlag = true;
                            player.moveFlag = false;
                        }
                        else if (selectAction == 2) // 移動時の処理
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

                        // 攻撃か移動
                        if (player.attackFlag || player.moveFlag)
                        {
                            bool[] actionResult = new bool[3];
                            //
                            // ターン終了に向けた処理
                            //
                            actionResult = select.AttackOrMove(player);
                            attacked = actionResult[0]; // 攻撃した場合
                            moved = actionResult[1];    // 移動した場合
                            isArea = actionResult[2];   // 選択範囲を未設定にする
                        }
                    }
                    else
                    {
                        // 相手ターンは、枠の色を初期状態に戻す
                        frameManager.InitColor(player);
                    }

                    // 攻撃時の処理
                    string[] shipsBlocks = new string[3];

                    // 敵戦艦の位置取得
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
                    // 攻撃対象を選択時の処理
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

                        // 攻撃が当たったときの処理
                        for (int j = 0; j < shipsBlocks.Length; j++)
                        {
                            if (player.attackObjName == shipsBlocks[j])
                            {
                                isHit = true;

                                // HP処理
                                player.opponentShipsHP[j]--;
                                player.hitFlag = true;

                                // ログ更新
                                string logText = attackOrMoveArea.GetHitLog(attackAreaIdx, player.playerName, j);
                                player.logText = GetLogText(player.logText, logText);
                                uIManager.DisplayLog(player.logText);
                                player.logUpdateFlag = true;

                                // 残基が0になったら破壊する船を設定する
                                if (player.opponentShipsHP[j] == 0)
                                {
                                    player.destroyedShipName = NameDefinition.SHIPLIST[j];
                                }
                                break;
                            }
                        }

                        // 攻撃が当たったときにゲーム終了判定を行う
                        if (player.hitFlag)
                        {
                            player.finishGameFlag = gameEnd.SetFinishGameFlag(player.opponentShipsHP);
                        }

                        // 攻撃がニアミスだった時の処理
                        if (!isHit)
                        {
                            string[] nearMissShips = new string[3];
                            // ニアミス対象戦艦の取得
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
                            // ログ更新
                            if (isNearMiss)
                            {
                                string logText = attackOrMoveArea.GetNearMissLog(attackAreaIdx, player.playerName, nearMissShips);
                                player.logText = GetLogText(player.logText, logText);
                                uIManager.DisplayLog(player.logText);
                                player.logUpdateFlag = true;
                            }
                        }

                        // 攻撃が当たらなかったときのログ更新
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
                    // 移動時の処理
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

                    // ターン終了時に次のプレイヤーを設定
                    turnManager.EndTurn(player);

                    // ゲーム終了の場合
                    if (player.finishGameFlag && !isFinish)
                    {
                        string finishText = "You win!!!";
                        uIManager.EndGame(finishText);
                        isFinish = true;
                    }

                    // 描画のため0.2s待ってから、処理完了フラグを下す
                    StartCoroutine(DelayMethod(0.2f, player));
                }
            }
            else
            {
                // 情報の更新
                for (int j = 0; j < players.Length; j++)
                {
                    GameObject tmpPlayerObj = players[j];
                    GamePlayer tmpPlayer = tmpPlayerObj.GetComponent<GamePlayer>();
                    if (tmpPlayer.id <= 2 && tmpPlayer.id != player.id)
                    {
                        // 攻撃が当たった場合、HPの更新
                        if (player.hitFlag)
                        {
                            tmpPlayer.hitFlag = true;
                            tmpPlayer.attackObjName = player.attackObjName;
                            for (int k = 0; k < player.shipsHP.Length; k++)
                            {
                                tmpPlayer.shipsHP[k] = player.opponentShipsHP[k];
                            }
                            tmpPlayer.destroyedShipName = player.destroyedShipName;
                            tmpPlayer.finishGameFlag = player.finishGameFlag;
                            player.hitFlag = false;
                        }
                        // ログの更新
                        if (player.logUpdateFlag)
                        {
                            tmpPlayer.logText = player.logText;
                            uIManager.DisplayLog(tmpPlayer.logText);
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
        //delay秒待つ
        yield return new WaitForSeconds(delay);
        /*処理*/
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
        //delay秒待つ
        yield return new WaitForSeconds(delay);
        /*処理*/
        select.AttackOrMove(player);
    }

    // ログ情報を取得
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

    // 戻るボタンを押したときにフラグを立てる
    public void SetBackOpeFlag()
    {
        backOpeFlag = true;
    }

    // 戻るボタンで戦艦の配置を一つ戻す
    public void backOperation(GamePlayer player)
    {
        if (player.selectPhase == 2)    // 戦艦配置キャンセル
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
        else if (player.selectPhase == 3)   // 駆逐艦配置キャンセル
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
        else if (player.selectPhase == 4)   // 潜水艦配置キャンセル
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

    // 配置完了ボタンを押したときに配置完了フラグを立てる
    public void DecideDeploy()
    {
        deployDoneFlag = true;
    }

    // 攻撃ボタンを押した場合の処理
    public void SelectAttackAction()
    {
        selectAction = 1;   // 攻撃
        selectedShip = false;   // 移動対象戦艦選択状態キャンセル
        isArea = false;     // 未使用
        isClear = false;    // 初期状態
    }

    // 移動ボタンを押した場合の処理
    public void SelectMoveAction()
    {
        selectAction = 2;   // 移動
        selectedShip = false;   // 移動対象戦艦選択状態キャンセル
        isArea = false;     // 未使用
        isClear = false;    // 初期状態
    }
}
