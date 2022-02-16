using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsernameDisplay : MonoBehaviour
{
    [SerializeField] private PhotonView _playerPV;
    [SerializeField] private TMP_Text _text;


    private void Start()
    {
        if (_playerPV.IsMine)
        {
            gameObject.SetActive(false);
        }

        _text.text = _playerPV.Owner.NickName;
    }
}
