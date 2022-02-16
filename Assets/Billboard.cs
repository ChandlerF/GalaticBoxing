using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera _cam;

    void Update()
    {
        if (_cam == null)
            _cam = FindObjectOfType<Camera>();

        if (_cam == null)
            return;

        transform.LookAt(_cam.transform);
        transform.Rotate(Vector3.up * 180);
    }
}
