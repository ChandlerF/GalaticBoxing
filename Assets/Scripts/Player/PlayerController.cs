using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _cameraHolder;

    [SerializeField] private float _mouseSensitivity = 2f, _sprintSpeed = 6f, _walkSpeed = 3f, _jumpForce = 250f, _smoothTime = 0.15f;

    private float _verticalLookRotation;

    private bool _grounded = true;
    private Vector3 _smoothMoveVelocity, _moveAmount;

    private Rigidbody _rb;
    private PhotonView _pv;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (!_pv.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(_rb);
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
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        _moveAmount = Vector3.SmoothDamp(_moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) && _grounded ? _sprintSpeed : _walkSpeed), ref _smoothMoveVelocity, _smoothTime);

    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _grounded)
        {
            _rb.AddForce(transform.up * _jumpForce);
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
}
