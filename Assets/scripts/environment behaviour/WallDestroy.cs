using System;
using UnityEngine;

public class WallDestroy : MonoBehaviour
{
    [SerializeField] float minSpeedToDestroyWall = 0.1f;

    public bool TryDestroyWall(float speed)
    {
        if (speed > minSpeedToDestroyWall)
        {
            return true;
        }
        return false;
    }
}
