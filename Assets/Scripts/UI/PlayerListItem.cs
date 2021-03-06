using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text _text;
    private Player _player;


    private void Awake()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            RoomManager.Instance.SetTeam(1);
        }
        else
        {
            RoomManager.Instance.SetTeam(0);
        }
    }

    public void SetUp(Player player)
    {
        _player = player;
        _text.text = player.NickName;

        Invoke("TextToTeamColor", 0.1f);
    }

    private void TextToTeamColor()
    {
        int team = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
        if (team == 0) { _text.color = Color.green; }
        else if (team == 1) { _text.color = Color.red; }
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(_player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }



    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
