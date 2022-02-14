using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    private PlayerController _playerController;

    void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
    }


    private void OnTriggerEnter(Collider col)
    {
        _playerController.SetGroundedState(true);
    }


    private void OnTriggerExit(Collider col)
    {
        _playerController.SetGroundedState(false);
    }

    private void OnTriggerStay(Collider col)
    {
        _playerController.SetGroundedState(true);
    }
}
