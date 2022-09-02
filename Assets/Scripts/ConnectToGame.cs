using Photon.Pun;
//using Photon.Realtime;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにする
public class ConnectToGame : MonoBehaviourPunCallbacks
{
    public TMP_InputField playernameCreateInputField;
    public TMP_InputField playernameJoinInputField;
    public Toggle hideSubmarineToggle;
    public Toggle noteToggle;

    private bool ishide;
    private bool isnote;

    private void Awake()
    {
        // プレイヤー自身の名前を"Player"に設定する
        PhotonNetwork.NickName = "Player";

        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();

        PhotonNetwork.SendRate = 1; // 1秒間にメッセージ送信を行う回数
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    //public override void OnConnectedToMaster()
    //{
    //    // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
    //    PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    //}

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        // ランダムな座標に自身のアバター（ネットワークオブジェクト）を生成する
        //var position = new Vector3(Random.Range(3f, 5f), Random.Range(-5f, -3f));
        //GameObject g = PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
        //GamePlayer player = g.GetComponent<GamePlayer>();
        //if (GameSettings.createOrJoin == 1)
        //{
        //    player.playerName = playernameCreateInputField.text;
        //}
        //else if (GameSettings.createOrJoin == 2)
        //{
        //    player.playerName = playernameJoinInputField.text;
        //}
        ////player.hideSubmarineOpt = hideSubmarineToggle.isOn;
        ////player.noteOpt = noteToggle.isOn;
        ////UnityEngine.Debug.Log(hideSubmarineToggle.isOn);
        //ishide = hideSubmarineToggle.isOn;
        //if (ishide)
        //{
        //    UnityEngine.Debug.Log(hideSubmarineToggle.isOn);
        //    player.hideSubmarineOpt = 1;
        //}
        //else
        //{
        //    player.hideSubmarineOpt = 2;
        //}
        //if (noteToggle.isOn)
        //{
        //    player.noteOpt = 1;
        //}
        //else
        //{
        //    player.noteOpt = 2;
        //}
    }
}
