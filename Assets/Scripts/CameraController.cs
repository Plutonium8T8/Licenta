using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Camera _camera;

    [SerializeField] public float cameraSpeed = 0.001f;

    [SerializeField] public float cameraMovementRatio = 0.001f;

    private Vector2 _mousePosition;

    private Vector2 cameraMovePosition;

    private void Update()
    {
        _mousePosition = _camera.ScreenToViewportPoint(Input.mousePosition);
        Debug.Log(_mousePosition);
    }

    private void FixedUpdate()
    {
        if (_mousePosition.x > 0.90 && _camera.transform.position.x <= 30)
        {
            cameraMovePosition = _camera.transform.position;
            cameraMovePosition.x += cameraMovementRatio * Time.deltaTime;
            _camera.transform.position = cameraMovePosition;
            _camera.transform.position += new Vector3(_camera.transform.position.x, _camera.transform.position.y, -10);
        }
        else if (_mousePosition.x < 0.10 && _camera.transform.position.x >= -30)
        {
            cameraMovePosition = _camera.transform.position;
            cameraMovePosition.x -= cameraMovementRatio * Time.deltaTime;
            _camera.transform.position = cameraMovePosition;
            _camera.transform.position += new Vector3(_camera.transform.position.x, _camera.transform.position.y, -10);
        }


        if (_mousePosition.y > 0.90 && _camera.transform.position.y <= 20)
        {
            cameraMovePosition = _camera.transform.position;
            cameraMovePosition.y += cameraMovementRatio * Time.deltaTime;
            _camera.transform.position = cameraMovePosition;
            _camera.transform.position += new Vector3(_camera.transform.position.x, _camera.transform.position.y, -10);
        }
        else if (_mousePosition.y < 0.10 && _camera.transform.position.y >= -20)
        {
            cameraMovePosition = _camera.transform.position;
            cameraMovePosition.y -= cameraMovementRatio * Time.deltaTime;
            _camera.transform.position = cameraMovePosition;
            _camera.transform.position += new Vector3(_camera.transform.position.x, _camera.transform.position.y, -10);
        }
    }
}
