using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{
    [SerializeField] private Image _healthBarImage;
    [SerializeField] private GameObject _ui;

    [SerializeField] private GameObject _cameraHolder;

    [SerializeField] private float _mouseSensitivity = 2f, _sprintSpeed = 6f, _walkSpeed = 3f, _jumpForce = 250f, _smoothTime = 0.15f;

    private float _verticalLookRotation;

    private bool _grounded = true;
    private Vector3 _smoothMoveVelocity, _moveAmount;

    [SerializeField] private Item[] _items;
    private int _itemIndex;
    private int _previousItemIndex = -1;


    private Rigidbody _rb;
    private PhotonView _pv;

    private const float _maxHealth = 100f;
    private float _currentHealth = _maxHealth;

    private PlayerManager _playerManager;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _pv = GetComponent<PhotonView>();

        _playerManager = PhotonView.Find((int)_pv.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    private void Start()
    {
        if (_pv.IsMine)
        {
            EquipItem(0);



            /*
            int team = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            Debug.Log(team);
            if (team == 0) { _mouseSensitivity = 0f; }
            else { _mouseSensitivity = 100f; }*/
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(_rb);
            Destroy(_ui);
        }


        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (!_pv.IsMine)
            return;


        Look();
        Move();
        Jump();

        for (int i = 0; i < _items.Length; i++)
        {
            if(Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }

        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if(_itemIndex >= _items.Length - 1)
            {
                EquipItem(0);
            }
            else
            {
                EquipItem(_itemIndex + 1);
            }
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if (_itemIndex <= 0)
            {
                EquipItem(_items.Length - 1);
            }
            else
            {
                EquipItem(_itemIndex - 1);
            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            _items[_itemIndex].Use();
        }

        //Die when you fall off the map
        if(transform.position.y < -10f)
        {
            Die();
        }
    }



    private void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * _mouseSensitivity);

        _verticalLookRotation += Input.GetAxisRaw("Mouse Y") * _mouseSensitivity;
        _verticalLookRotation = Mathf.Clamp(_verticalLookRotation, -90f, 90f);

        _cameraHolder.transform.localEulerAngles = Vector3.left * _verticalLookRotation;
    }

    private void Move()
    {
        Vector3 moveDir = Vector3.zero;
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        _moveAmount = Vector3.SmoothDamp(_moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) && _grounded ? _sprintSpeed : _walkSpeed), ref _smoothMoveVelocity, _smoothTime);

    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _grounded)
        {
            _rb.AddForce(transform.up * _jumpForce);
        }
    }

    private void EquipItem(int index)
    {
        if (index == _previousItemIndex)
            return;

        _itemIndex = index;

        _items[_itemIndex].ItemGameObject.SetActive(true);

        if(_previousItemIndex != -1)
        {
            _items[_previousItemIndex].ItemGameObject.SetActive(false);
        }

        _previousItemIndex = _itemIndex;

        if (_pv.IsMine)
        {
            
            Hashtable hash = new Hashtable();
            //Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;
            hash.Add("itemIndex", _itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }


    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(!_pv.IsMine && targetPlayer == _pv.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
        }
    }


    public void SetGroundedState(bool grounded)
    {
        _grounded = grounded;
    }


    private void FixedUpdate()
    {
        if (!_pv.IsMine)
            return;

        _rb.MovePosition(_rb.position + transform.TransformDirection(_moveAmount) * Time.fixedDeltaTime);
    }


    //Code runs on Shooter's computer
    public void TakeDamage(float damage)
    {
        _pv.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }


    //Code runs on everyones computer, but only victim is affected, because they have _pv.IsMine = true
    [PunRPC]
    private void RPC_TakeDamage(float damage)
    {
        if (!_pv.IsMine) { return;}

        _currentHealth -= damage;

        _healthBarImage.fillAmount = _currentHealth / _maxHealth;

        if (_currentHealth <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        _playerManager.Die();
    }
}
