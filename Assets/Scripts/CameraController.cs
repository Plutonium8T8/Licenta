using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour
{
    private Func<Vector3> GetCameraPositionFunc;

    private Func<float> GetCameraZoomFunc;

    private Camera _camera;

    [SerializeField] private float cameraZoomSpeed = 50f;

    [SerializeField] private float cameraMoveSpeed = 20f;

    private void Start()
    {
        _camera = transform.GetComponent<Camera>();
    }

    public void Setup(Func<Vector3> GetCameraPositionFunc, Func<float> GetCameraZoomFunc)
    {
        this.GetCameraPositionFunc = GetCameraPositionFunc;
        this.GetCameraZoomFunc = GetCameraZoomFunc;
    }

    public void OnCameraMovement(Func<Vector3> GetCameraPositionFunc)
    {
        this.GetCameraPositionFunc = GetCameraPositionFunc;
    }

    private void Update()
    {
        float zoom = GetCameraZoomFunc();

        float zoomDifference = zoom - _camera.orthographicSize;

        _camera.orthographicSize += zoomDifference * cameraZoomSpeed * Time.deltaTime;

        Vector3 cameraPosition = GetCameraPositionFunc();
        cameraPosition.z = transform.position.z;

        Vector3 cameraMoveDirection = (cameraPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, cameraPosition);

        if (distance > 0)
        {
            Vector3 newCameraPosition = transform.position + cameraMoveDirection * distance * cameraMoveSpeed * Time.deltaTime;

            float distanceAfterMovingg = Vector3.Distance(newCameraPosition, cameraPosition);

            if (distanceAfterMovingg > distance)
            {
                newCameraPosition = cameraPosition;
            }

            transform.position = newCameraPosition;
        }      
    }
}
