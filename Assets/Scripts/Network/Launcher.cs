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
    [SerializeField] Transform _playerListContent;
    [SerializeField] GameObject _playerListItemPrefab;
    [SerializeField] GameObject _startGameButton, _swapTeamsButton;

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
        PhotonNetwork.AutomaticallySyncScene = true;
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

        SetUpPlayerListItems();

        _startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        _swapTeamsButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void SetUpPlayerListItems()
    {
        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in _playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(_playerListItemPrefab, _playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        _startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        _swapTeamsButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed Joining Room");

        _errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("ErrorMenu");
    }

    public void StartGame()
    {
        //1 because it's the scene's build index
        PhotonNetwork.LoadLevel(1);
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);

        RoomManager.Instance.CanChangeTeams = true;

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
            if (roomList[i].RemovedFromList)
            {
                continue;
            }

            Instantiate(_roomListItemPrefab, _roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(_playerListItemPrefab, _playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }
}
