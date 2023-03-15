using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Utils
{
    public static class UtilsClass
    {
        public static Vector2 GetMouseWorldPosition()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        public static Vector3 GetRandomDirection()
        {
            return new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        }
    }
}

