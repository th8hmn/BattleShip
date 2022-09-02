using Photon.Pun;
//using Photon.Realtime;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// MonoBehaviourPunCallbacks���p�����āAPUN�̃R�[���o�b�N���󂯎���悤�ɂ���
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
        // �v���C���[���g�̖��O��"Player"�ɐݒ肷��
        PhotonNetwork.NickName = "Player";

        // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.ConnectUsingSettings();

        PhotonNetwork.SendRate = 1; // 1�b�ԂɃ��b�Z�[�W���M���s����
    }

    // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    //public override void OnConnectedToMaster()
    //{
    //    // "Room"�Ƃ������O�̃��[���ɎQ������i���[�������݂��Ȃ���΍쐬���ĎQ������j
    //    PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    //}

    // �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnJoinedRoom()
    {
        // �����_���ȍ��W�Ɏ��g�̃A�o�^�[�i�l�b�g���[�N�I�u�W�F�N�g�j�𐶐�����
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
