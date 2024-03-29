using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchmakingView : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_InputField passwordInputField = default;
    [SerializeField]
    private TMP_InputField playerNameInputField = default;
    [SerializeField]
    private Button joinRoomButton = default;

    private CanvasGroup canvasGroup;

    [SerializeField]
    private GameObject createRoomView = default;
    [SerializeField]
    private GameObject switchUI = default;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        // マスターサーバーに接続するまでは、入力できないようにする
        canvasGroup.interactable = false;

        // パスワードを入力する前は、ルーム参加ボタンを押せないようにする
        joinRoomButton.interactable = false;

        passwordInputField.onValueChanged.AddListener(OnPasswordInputFieldValueChanged);
        playerNameInputField.onValueChanged.AddListener(OnPlayerNmaeInputFieldValueChanged);
        joinRoomButton.onClick.AddListener(OnJoinRoomButtonClick);
    }

    public override void OnConnectedToMaster()
    {
        // マスターサーバーに接続したら、入力できるようにする
        canvasGroup.interactable = true;
    }

    private void OnPasswordInputFieldValueChanged(string value)
    {
        // パスワードを1桁以上入力した時のみ、ルーム参加ボタンを押せるようにする
        joinRoomButton.interactable = (value.Length > 0);
        joinRoomButton.interactable = (playerNameInputField.text.Length > 0);
    }

    private void OnPlayerNmaeInputFieldValueChanged(string value)
    {
        // パスワードを1桁以上入力した時のみ、ルーム参加ボタンを押せるようにする
        joinRoomButton.interactable = (value.Length > 0);
        joinRoomButton.interactable = (passwordInputField.text.Length > 0);
    }

    private void OnJoinRoomButtonClick()
    {
        // ルーム参加処理中は、入力できないようにする
        canvasGroup.interactable = false;

        // ルームを非公開に設定する（新規でルームを作成する場合）
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsVisible = false;

        // パスワードと同じ名前のルームに参加する（ルームが存在しなければ作成してから参加する）
        //PhotonNetwork.JoinOrCreateRoom(passwordInputField.text, roomOptions, TypedLobby.Default);
        // パスワードと同じ名前のルームに参加する
        PhotonNetwork.JoinRoom(passwordInputField.text);
        GameSettings.createOrJoin = 2;
    }

    public override void OnJoinedRoom()
    {
        // ランダムな座標に自身のアバター（ネットワークオブジェクト）を生成する
        var position = new Vector3(Random.Range(3f, 5f), Random.Range(-5f, -3f));
        GameObject g = PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
        GamePlayer player = g.GetComponent<GamePlayer>();
        player.playerName = playerNameInputField.text;

        // ルームへの参加が成功したら、UIを非表示にする
        gameObject.SetActive(false);
        createRoomView.SetActive(false);
        switchUI.SetActive(false);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // ルームへの参加が失敗したら、パスワードを再び入力できるようにする
        passwordInputField.text = string.Empty;
        canvasGroup.interactable = true;
        GameSettings.createOrJoin = 0;
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public void Reconnect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.CurrentRoom.PlayerTtl = 30000;
            PhotonNetwork.ReconnectAndRejoin();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }
}