using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeReference] private TMP_Text _text;

    private RoomInfo _info;

    public void SetUp(RoomInfo info)
    {
        _info = info;

        _text.text = info.Name;
    }

    public void OnClick()
    {
        Launcher.Instance.JoinRoom(_info);
    }
}
