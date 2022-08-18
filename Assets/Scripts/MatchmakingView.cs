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

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        // �}�X�^�[�T�[�o�[�ɐڑ�����܂ł́A���͂ł��Ȃ��悤�ɂ���
        canvasGroup.interactable = false;

        // �p�X���[�h����͂���O�́A���[���Q���{�^���������Ȃ��悤�ɂ���
        joinRoomButton.interactable = false;

        passwordInputField.onValueChanged.AddListener(OnPasswordInputFieldValueChanged);
        playerNameInputField.onValueChanged.AddListener(OnPlayerNmaeInputFieldValueChanged);
        joinRoomButton.onClick.AddListener(OnJoinRoomButtonClick);
    }

    public override void OnConnectedToMaster()
    {
        // �}�X�^�[�T�[�o�[�ɐڑ�������A���͂ł���悤�ɂ���
        canvasGroup.interactable = true;
    }

    private void OnPasswordInputFieldValueChanged(string value)
    {
        // �p�X���[�h��1���ȏ���͂������̂݁A���[���Q���{�^����������悤�ɂ���
        joinRoomButton.interactable = (value.Length > 0);
        joinRoomButton.interactable = (playerNameInputField.text.Length > 0);
    }

    private void OnPlayerNmaeInputFieldValueChanged(string value)
    {
        // �p�X���[�h��1���ȏ���͂������̂݁A���[���Q���{�^����������悤�ɂ���
        joinRoomButton.interactable = (value.Length > 0);
        joinRoomButton.interactable = (passwordInputField.text.Length > 0);
    }

    private void OnJoinRoomButtonClick()
    {
        // ���[���Q���������́A���͂ł��Ȃ��悤�ɂ���
        canvasGroup.interactable = false;

        // ���[�������J�ɐݒ肷��i�V�K�Ń��[�����쐬����ꍇ�j
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsVisible = false;

        // �p�X���[�h�Ɠ������O�̃��[���ɎQ������i���[�������݂��Ȃ���΍쐬���Ă���Q������j
        PhotonNetwork.JoinOrCreateRoom(passwordInputField.text, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        // ���[���ւ̎Q��������������AUI���\���ɂ���
        gameObject.SetActive(false);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // ���[���ւ̎Q�������s������A�p�X���[�h���Ăѓ��͂ł���悤�ɂ���
        passwordInputField.text = string.Empty;
        canvasGroup.interactable = true;
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