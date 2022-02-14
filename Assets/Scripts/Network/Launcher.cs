using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] private TMP_InputField _roomNameInputField;
    [SerializeField] private TMP_Text _errorText;
    [SerializeField] private TMP_Text _roomNameText;
    [SerializeField] Transform _roomListContent;
    [SerializeField] GameObject _roomListItemPrefab;

    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        MenuManager.Instance.OpenMenu("TitleMenu");
    }


    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(_roomNameInputField.text))
        {
            return;
        }

        PhotonNetwork.CreateRoom(_roomNameInputField.text);

        MenuManager.Instance.OpenMenu("LoadingMenu");
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");

        MenuManager.Instance.OpenMenu("RoomMenu");
        _roomNameText.text = PhotonNetwork.CurrentRoom.Name;
    }


    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed Joining Room");

        _errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("ErrorMenu");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);

        MenuManager.Instance.OpenMenu("LoadingMenu");
    }


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();

        MenuManager.Instance.OpenMenu("LoadingMenu");
    }



    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform trans in _roomListContent)
        {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            Instantiate(_roomListItemPrefab, _roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }
}
