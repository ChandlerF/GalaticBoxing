using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField] private SpawnPoint _agentSpawn, _assassinSpawn;

    private void Awake()
    {
        Instance = this;
    }


    public Transform GetSpawnPoint()
    {
        int team = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
        if (team == 0)
        {
            return _agentSpawn.transform;
        }
        else if(team == 1)
        {
            return _assassinSpawn.transform;
        }

        return null;
    }
}
