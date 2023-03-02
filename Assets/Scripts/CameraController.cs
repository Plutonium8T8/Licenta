using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour
{
    private Func<Vector3> GetCameraPositionFunc;

    public void Setup(Func<Vector3> GetCameraPositionFunc)
    {
        this.GetCameraPositionFunc = GetCameraPositionFunc;
    }

    private void Update()
    {
        Vector3 cameraPosition = GetCameraPositionFunc();
        cameraPosition.z = transform.position.z;
        transform.position = cameraPosition;
    }
}
