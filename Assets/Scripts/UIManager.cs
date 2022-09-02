using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject backBtn;
    public GameObject deployCompBtn;
    public GameObject attackBtn;
    public GameObject moveBtn;

    public GameObject pleaseWait;
    private Text pleaseWaitTxt;
    public GameObject instruct1;
    private Text instruct1Txt;
    public GameObject instruct2;
    private Text instruct2Txt;
    public GameObject instruct3;
    private Text instruct3Txt;
    public GameObject instruct4;
    private Text instruct4Txt;
    public GameObject instructPanelObj;

    public Text player1Name;
    public Text player2Name;
    public GameObject battleshipObj;
    public Text battleshipHP1Txt;
    public Text battleshipHP2Txt;
    public GameObject destroyerObj;
    public Text destroyerHP1Txt;
    public Text destroyerHP2Txt;
    public GameObject submarineObj;
    public Text submarineHP1Txt;
    public Text submarineHP2Txt;
    public GameObject panelObj;
    public GameObject HPTextObj;

    public GameObject logBaseObj;
    private GameObject logBaseObj2;
    public GameObject scrollViewObj;
    private ScrollRect scrollRect;
    private Text logTxt;
    public GameObject scrollViewObj2;
    private ScrollRect scrollRect2;
    private Text logTxt2;
    public GameObject playerNameForLogObj;
    public Text playerNameForLogTxt1;
    public Text playerNameForLogTxt2;

    public GameObject finishPanelObj;
    private Text finishTxt;

    // Start is called before the first frame update
    void Awake()
    {
        backBtn.SetActive(false);
        deployCompBtn.SetActive(false);
        attackBtn.SetActive(false);
        moveBtn.SetActive(false);

        pleaseWait.SetActive(true);
        pleaseWaitTxt = pleaseWait.GetComponent<Text>();
        instruct1.SetActive(false);
        instruct1Txt = instruct1.GetComponent<Text>();

        instruct2.SetActive(false);
        instruct2Txt = instruct2.GetComponent<Text>();

        instruct3.SetActive(false);
        instruct3Txt = instruct3.GetComponent<Text>();

        instruct4.SetActive(false);
        instruct4Txt = instruct4.GetComponent<Text>();

        instructPanelObj.SetActive(true);

        scrollViewObj.SetActive(false);
        GameObject viewPortObj = scrollViewObj.transform.GetChild(0).gameObject;
        scrollRect = scrollViewObj.GetComponent<ScrollRect>();
#if (!UNITY_WEBGL)
        logBaseObj2 = Instantiate(logBaseObj, new Vector3(3f, -3f, 0f), Quaternion.identity);
        GameObject tmp = logBaseObj2.transform.GetChild(0).gameObject;
        scrollViewObj2 = tmp.transform.GetChild(0).gameObject;
        scrollViewObj2.SetActive(false);
        GameObject viewPortObj2 = scrollViewObj2.transform.GetChild(0).gameObject;
        scrollRect2 = scrollViewObj2.GetComponent<ScrollRect>();
        logBaseObj.transform.position = new Vector3(-3.5f, -3f, 0f);
#endif

        GameObject logObj = viewPortObj.transform.GetChild(0).gameObject;
        logTxt = logObj.GetComponent<Text>();
#if (!UNITY_WEBGL)
        GameObject logObj2 = viewPortObj2.transform.GetChild(0).gameObject;
        logTxt2 = logObj2.GetComponent<Text>();
#endif
        playerNameForLogObj.SetActive(false);

        finishPanelObj.SetActive(false);
        GameObject finishTxtObj = finishPanelObj.transform.GetChild(0).gameObject;
        finishTxt = finishTxtObj.GetComponent<Text>();

        float i_ = 0f;
        for (int i = 0; i < 2; i++)
        {
            if (i == 0)
            {
                i_ = -1f;
            }
            else if (i == 1)
            {
                i_ = 1f;
            }

            Vector3 battleshipDispPos = new Vector3(-0.5f, 0.2f * i_, 0f);
            GameObject g1 = Instantiate(battleshipObj, battleshipDispPos, Quaternion.identity, panelObj.transform);
            g1.name = NameDefinition.BATTLESHIP + "_HP";
            g1.transform.localScale = g1.transform.localScale * 30;
            BoxCollider2D boxCollider2D_g1 = g1.GetComponent<BoxCollider2D>();
            Destroy(boxCollider2D_g1);

            Vector3 destroyerDispPos = new Vector3(0.3f, 0.2f * i_, 0f);
            GameObject g2 = Instantiate(destroyerObj, destroyerDispPos, Quaternion.identity, panelObj.transform);
            g2.name = NameDefinition.DESTROYER + "_HP";
            g2.transform.localScale = g2.transform.localScale * 30;
            BoxCollider2D boxCollider2D_g2 = g2.GetComponent<BoxCollider2D>();
            Destroy(boxCollider2D_g2);

            Vector3 submarineDispPos = new Vector3(1.1f, 0.2f * i_, 0f);
            GameObject g3 = Instantiate(submarineObj, submarineDispPos, Quaternion.identity, panelObj.transform);
            g3.name = NameDefinition.SUBMARINE + "_HP";
            g3.transform.localScale = g3.transform.localScale * 30;
            BoxCollider2D boxCollider2D_g3 = g3.GetComponent<BoxCollider2D>();
            Destroy(boxCollider2D_g3);
        }

        HPTextObj.transform.position = new Vector3(-0.25f, 4.25f, 0f);
        HPTextObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 戦艦配置フェーズ
    public void GoSelectPhase1()
    {
        // 進んだときに表示を変更
        pleaseWait.SetActive(false);
        instruct1.SetActive(true);

        // 戻ったときに非表示にする
        instruct2.SetActive(false);
        backBtn.SetActive(false);
    }

    public void DisplayInstruct1(GamePlayer player, string rand)
    {
        instruct1Txt.text = "myturnFlag: " + Convert.ToString(player.myturnFlag) + "\n"
            + "; turnFinishFlag: " + Convert.ToString(player.turnFinishFlag) + "\n"
            + "; id: " + Convert.ToString(player.id) + "\n"
            + "; myturnPlayerID: " + Convert.ToString(player.myturnPlayerID) + "\n"
            + "; battloshipIdx: (" + Convert.ToString(player.battleshipIdx[0]) + "\n"
            + "," + Convert.ToString(player.battleshipIdx[1]) + "\n"
            + "; destroyerIdx: (" + Convert.ToString(player.destroyerIdx[0]) + "\n"
            + "," + Convert.ToString(player.destroyerIdx[1]) + "\n"
            + "; submarineIdx: (" + Convert.ToString(player.submarineIdx[0]) + "\n"
            + "," + Convert.ToString(player.submarineIdx[1]) + "\n"
            + ")    ; attackObjName: " + Convert.ToString(player.attackObjName) + "\n"
            + "; shipsBolcks[0]: " + rand + "\n"
            + "; shipsHP: " + Convert.ToString(player.shipsHP[0]) + "\n"
            + "; opponentShipsHP: " + Convert.ToString(player.opponentShipsHP[0]) + "\n"
            + "; logText: " + Convert.ToString(player.logText) + "\n"
            + "; attackFlag: " + Convert.ToString(player.attackFlag) + "\n"
            + "; moveFlag: " + Convert.ToString(player.moveFlag) + "\n"
            + "; shipName: " + Convert.ToString(player.selectShipName) + "\n"
            + "; selectPhase: " + Convert.ToString(player.selectPhase) + "\n"
            + "; hitFlag: " + Convert.ToString(player.hitFlag);
    }

    // 駆逐艦配置フェーズ
    public void GoSelectPhase2()
    {
        // 進んだときに表示を変更
        instruct1.SetActive(false);
        instruct2.SetActive(true);
        backBtn.SetActive(true);

        // 戻ったときに非表示にする
        instruct3.SetActive(false);
    }

    // 潜水艦配置フェーズ
    public void GoSelectPhase3()
    {
        // 進んだときに表示を変更
        instruct2.SetActive(false);
        instruct3.SetActive(true);

        // 戻ったときに非表示にする
        instruct4.SetActive(false);
        deployCompBtn.SetActive(false);
    }

    // 配置完了確認フェーズ
    public void GoSelectPhase4()
    {
        instruct3.SetActive(false);

        instruct4.SetActive(true);

        deployCompBtn.SetActive(true);
    }

    public void DisplayInstruct2(GamePlayer player)
    {
        instruct2Txt.text = "myturnFlag: " + Convert.ToString(player.myturnFlag)
            + "; turnFinishFlag: " + Convert.ToString(player.turnFinishFlag)
            + "; id: " + Convert.ToString(player.id)
            + "; myturnPlayerID: " + Convert.ToString(player.myturnPlayerID);
    }

    // HP表示
    public void DisplayHP(GamePlayer player)
    {
        battleshipHP1Txt.text = " : " + player.shipsHP[0];
        destroyerHP1Txt.text = " : " + player.shipsHP[1];
        submarineHP1Txt.text = " : " + player.shipsHP[2];
        battleshipHP2Txt.text = " : " + player.opponentShipsHP[0];
        destroyerHP2Txt.text = " : " + player.opponentShipsHP[1];
        submarineHP2Txt.text = " : " + player.opponentShipsHP[2];
    }

    // HP表示の名前を取得
    public void SetPlayerName(GameObject[] players, int myplayerId)
    {
        for (int i = 0; i < players.Length; i++)
        {
            GameObject playerObj = players[i];
            GamePlayer player = playerObj.GetComponent<GamePlayer>();
            if (player.id <= 2 && myplayerId <= 2)
            {
                if (player.id == myplayerId)
                {
                    player1Name.text = player.playerName;
                }
                else
                {
                    player2Name.text = player.playerName;
                }
            }
            else if (player.id <= 2 && myplayerId > 2)
            {
                if (player.id == 1)
                {
                    player1Name.text = player.playerName;
                }
                else
                {
                    player2Name.text = player.playerName;
                }
            }
        }
    }

    // ログの登録
    public void DisplayLog(string logText)
    {
        logTxt.text = logText;
        logTxt.GetComponent<ContentSizeFitter>().SetLayoutVertical();
        scrollRect.verticalNormalizedPosition = 0;
    }

#if (!UNITY_WEBGL)
    // ログの登録
    public void DisplayLog2(string logText)
    {
        logTxt2.text = logText;
        logTxt2.GetComponent<ContentSizeFitter>().SetLayoutVertical();
        scrollRect2.verticalNormalizedPosition = 0;
    }
#endif

    // 配置完了後、ゲーム開始状態に遷移
    public void GoActionPhase(int playerId)
    {
        backBtn.SetActive(false);
        deployCompBtn.SetActive(false);
        instruct4.SetActive(false);
        instructPanelObj.SetActive(false);
        pleaseWaitTxt.text = player2Name.text + "'s " + "Turn";
        HPTextObj.SetActive(true);
        if (playerId > 2)  // ゲーム開始時、3人目以降の場合、ログ/HP以外消す
        {
            instruct1.SetActive(false);
            instructPanelObj.SetActive(false);
        }
        //else                // ゲーム開始時、3人目以降の場合、ログ以外消す
        //{
        //    instruct1.SetActive(false);
        //    instructPanelObj.SetActive(false);
        //}
        //hpDispObj.SetActive(true);
        scrollViewObj.SetActive(true);
#if (!UNITY_WEBGL)
        scrollViewObj2.SetActive(true);
        playerNameForLogTxt1.text = player1Name.text;
        playerNameForLogTxt2.text = player2Name.text;
        playerNameForLogObj.SetActive(true);
#endif
    }

    // ターン開始時に、攻撃・移動ボタンを表示
    public void StartTurn()
    {
        pleaseWait.SetActive(false);
        attackBtn.SetActive(true);
        moveBtn.SetActive(true);
    }

    // ターン終了時に、攻撃・移動ボタンを非表示
    public void FinishTurn(bool isFinish)
    {
        attackBtn.SetActive(false);
        moveBtn.SetActive(false);
        if (!isFinish)
        {
            pleaseWait.SetActive(true);
        }
    }

    // エフェクト表示前に相手ターンであることの表示を隠す
    public void HideWaitText()
    {
        pleaseWait.SetActive(false);
    }

    // ゲーム終了時、ゲーム終了のテキストを表示
    public void EndGame(string finishText)
    {
        finishPanelObj.SetActive(true);
        finishTxt.text = finishText;
    }
}
