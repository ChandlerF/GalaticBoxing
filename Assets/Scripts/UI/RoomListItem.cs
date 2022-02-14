using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeReference] private TMP_Text _text;

    public RoomInfo Info;

    public void SetUp(RoomInfo info)
    {
        Info = info;

        _text.text = info.Name;
    }

    public void OnClick()
    {
        Launcher.Instance.JoinRoom(Info);
    }
}
