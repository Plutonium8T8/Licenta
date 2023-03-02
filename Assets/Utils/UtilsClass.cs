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
    }
}

