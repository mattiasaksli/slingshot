using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{
    public static void Clamp(this Vector3 vector, Vector3 min, Vector3 max)
    {
        // max check
        if (vector.x > max.x) vector.x = max.x;
        if (vector.y > max.y) vector.y = max.y;
        if (vector.z > max.z) vector.z = max.z;
        // min check
        if (vector.x < min.x) vector.x = min.x;
        if (vector.y < min.y) vector.y = min.y;
        if (vector.z < min.z) vector.z = min.z;
    }
}