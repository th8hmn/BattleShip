using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomView : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_InputField roomInputField = default;
    [SerializeField]
    private TMP_InputField playerNameInputField = default;
    [SerializeField]
    private Button createRoomButton = default;
    [SerializeField]
    private Toggle hideSubmarineToggle = default;
    [SerializeField]
    private Toggle noteToggle = default;

    private CanvasGroup canvasGroup;

    [SerializeField]
    private GameObject joinRoomView = default;
    [SerializeField]
    private GameObject switchUI = default;
    [SerializeField]
    private GameObject noteToggleObj = default;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        // マスターサーバーに接続するまでは、入力できないようにする
        canvasGroup.interactable = false;

        // パスワードを入力する前は、ルーム参加ボタンを押せないようにする
        createRoomButton.interactable = false;

        roomInputField.onValueChanged.AddListener(OnRoomInputFieldValueChanged);
        playerNameInputField.onValueChanged.AddListener(OnPlayerNmaeInputFieldValueChanged);
        createRoomButton.onClick.AddListener(OnCreateRoomButtonClick);

#if (UNITY_WEBGL)
        noteToggleObj.SetActive(false);
#endif
    }

    public override void OnConnectedToMaster()
    {
        // マスターサーバーに接続したら、入力できるようにする
        canvasGroup.interactable = true;
    }

    private void OnRoomInputFieldValueChanged(string value)
    {
        // パスワードを1桁以上入力した時のみ、ルーム参加ボタンを押せるようにする
        createRoomButton.interactable = (value.Length > 0);
        createRoomButton.interactable = (playerNameInputField.text.Length > 0);
    }

    private void OnPlayerNmaeInputFieldValueChanged(string value)
    {
        // パスワードを1桁以上入力した時のみ、ルーム参加ボタンを押せるようにする
        createRoomButton.interactable = (value.Length > 0);
        createRoomButton.interactable = (roomInputField.text.Length > 0);
    }

    private void OnCreateRoomButtonClick()
    {
        // ルーム参加処理中は、入力できないようにする
        canvasGroup.interactable = false;

        // ルームを非公開に設定する（新規でルームを作成する場合）
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsVisible = false;

        // パスワードと同じ名前のルームに参加する（ルームが存在しなければ作成してから参加する）
        PhotonNetwork.CreateRoom(roomInputField.text, roomOptions, TypedLobby.Default);
        GameSettings.createOrJoin = 1;
    }

    public override void OnJoinedRoom()
    {
        // ランダムな座標に自身のアバター（ネットワークオブジェクト）を生成する
        var position = new Vector3(Random.Range(3f, 5f), Random.Range(-5f, -3f));
        GameObject g = PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
        GamePlayer player = g.GetComponent<GamePlayer>();
        player.playerName = playerNameInputField.text;
        if (hideSubmarineToggle.isOn)
        {
            GameSettings.isHideSubmarine = true;
        }
        //else
        //{
        //    player.hideSubmarineOpt = 2;
        //}
        if (noteToggle.isOn)
        {
            GameSettings.isNote = true;
        }
        //else
        //{
        //    player.noteOpt = 2;
        //}

        // ルームへの参加が成功したら、UIを非表示にする
        gameObject.SetActive(false);
        joinRoomView.SetActive(false);
        switchUI.SetActive(false);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        // ルームへの参加が失敗したら、パスワードを再び入力できるようにする
        roomInputField.text = string.Empty;
        canvasGroup.interactable = true;
        GameSettings.createOrJoin = 0;
    }
}
