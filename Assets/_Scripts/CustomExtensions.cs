using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomExtensions
{
    public static Vector3 NewX(this Vector3 vector3, float newX) {
        return new Vector3(newX, vector3.y, vector3.z);
    }

    public static Vector3 NewY(this Vector3 vector3, float newY)
    {
        return new Vector3(vector3.x, newY, vector3.z);
    }

    public static Vector3 NewZ(this Vector3 vector3, float newZ)
    {
        return new Vector3(vector3.x, vector3.y, newZ);
    }

}
