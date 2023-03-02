using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour
{
    private Func<Vector3> GetCameraPositionFunc;

    [SerializeField] private Camera _camera;

    public void Setup(Func<Vector3> GetCameraPositionFunc)
    {
        this.GetCameraPositionFunc = GetCameraPositionFunc;
    }

    private void Update()
    {
        /*Vector3 cameraPosition = new Vector3(2,2);
        cameraPosition.z = -20;
        _camera.transform.position = cameraPosition;*/
    }
}
