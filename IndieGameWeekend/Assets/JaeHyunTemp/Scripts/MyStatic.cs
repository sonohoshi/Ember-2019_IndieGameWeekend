using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyStatic
{
    public static Vector2 Rotate(this Vector2 v2, float degrees)
    {

        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v2.x;
        float ty = v2.y;

        v2.x = (cos * tx) - (sin * ty);
        v2.y = (sin * tx) + (cos * ty);

        return v2;
    }
}
