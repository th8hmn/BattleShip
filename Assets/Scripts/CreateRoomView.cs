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
        // �}�X�^�[�T�[�o�[�ɐڑ�����܂ł́A���͂ł��Ȃ��悤�ɂ���
        canvasGroup.interactable = false;

        // �p�X���[�h����͂���O�́A���[���Q���{�^���������Ȃ��悤�ɂ���
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
        // �}�X�^�[�T�[�o�[�ɐڑ�������A���͂ł���悤�ɂ���
        canvasGroup.interactable = true;
    }

    private void OnRoomInputFieldValueChanged(string value)
    {
        // �p�X���[�h��1���ȏ���͂������̂݁A���[���Q���{�^����������悤�ɂ���
        createRoomButton.interactable = (value.Length > 0);
        createRoomButton.interactable = (playerNameInputField.text.Length > 0);
    }

    private void OnPlayerNmaeInputFieldValueChanged(string value)
    {
        // �p�X���[�h��1���ȏ���͂������̂݁A���[���Q���{�^����������悤�ɂ���
        createRoomButton.interactable = (value.Length > 0);
        createRoomButton.interactable = (roomInputField.text.Length > 0);
    }

    private void OnCreateRoomButtonClick()
    {
        // ���[���Q���������́A���͂ł��Ȃ��悤�ɂ���
        canvasGroup.interactable = false;

        // ���[�������J�ɐݒ肷��i�V�K�Ń��[�����쐬����ꍇ�j
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsVisible = false;

        // �p�X���[�h�Ɠ������O�̃��[���ɎQ������i���[�������݂��Ȃ���΍쐬���Ă���Q������j
        PhotonNetwork.CreateRoom(roomInputField.text, roomOptions, TypedLobby.Default);
        GameSettings.createOrJoin = 1;
    }

    public override void OnJoinedRoom()
    {
        // �����_���ȍ��W�Ɏ��g�̃A�o�^�[�i�l�b�g���[�N�I�u�W�F�N�g�j�𐶐�����
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

        // ���[���ւ̎Q��������������AUI���\���ɂ���
        gameObject.SetActive(false);
        joinRoomView.SetActive(false);
        switchUI.SetActive(false);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        // ���[���ւ̎Q�������s������A�p�X���[�h���Ăѓ��͂ł���悤�ɂ���
        roomInputField.text = string.Empty;
        canvasGroup.interactable = true;
        GameSettings.createOrJoin = 0;
    }
}
