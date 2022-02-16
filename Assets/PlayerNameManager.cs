using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _usernameInput;


    private void Start()
    {
        if (PlayerPrefs.HasKey("Username"))
        {
            _usernameInput.text = PlayerPrefs.GetString("Username");
            PhotonNetwork.NickName = PlayerPrefs.GetString("Username");
        }
        else
        {
            _usernameInput.text = "Player " + Random.Range(0, 10000).ToString("0000");
            OnUsernameInputValueChanged();
        }
    }

    public void OnUsernameInputValueChanged()
    {
        PhotonNetwork.NickName = _usernameInput.text;
        PlayerPrefs.SetString("Username", _usernameInput.text);
    }
}
