using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomManager : MonoBehaviourPunCallbacks

{
    public static RoomManager Instance;
    public bool CanChangeTeams = true;
    public int Team = 0;
    private PhotonView _pv;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }

        _pv = GetComponent<PhotonView>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        //Build Index 1 is Game Scene
        if(scene.buildIndex == 1)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
    }

    public void SetTeam(int team)
    {
        if (CanChangeTeams)
        {
            Team = team;

            Hashtable hash = new Hashtable();
            hash.Add("Team", Team);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);


            CanChangeTeams = false;
        }
    }


    public void SwapTeams()
    {
        _pv.RPC("RPC_SwapTeams", RpcTarget.All);
        Launcher.Instance.SetUpPlayerListItems();
    }

    [PunRPC]
    private void RPC_SwapTeams()
    {
        CanChangeTeams = true;

        if(Team == 0) {SetTeam(1);}
        else if(Team == 1) {SetTeam(0);}
    }
}
