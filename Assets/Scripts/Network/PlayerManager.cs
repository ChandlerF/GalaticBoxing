using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    private PhotonView _pv;

    void Awake()
    {
        _pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (_pv.IsMine)
        {
            CreateController();
        }
    }


    private void CreateController()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity);
    }
}
