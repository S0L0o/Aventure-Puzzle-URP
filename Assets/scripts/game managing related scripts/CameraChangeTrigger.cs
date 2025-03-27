using System;
using Unity.VisualScripting;
using UnityEngine;

public class CameraChangeTrigger : MonoBehaviour
{
    private CameraFollow cameraFollow;
    public Transform newStaticCamPos;
    public bool followPlayer = false;
    private Vector3 offsetIfFollowPlayer;
    public float camSpeedOnZoneEntering = 0.5f;
    public float camSpeedOnZoneExiting = 0.125f;

    private void Awake()
    {
        offsetIfFollowPlayer = newStaticCamPos.position - transform.position;
        cameraFollow = FindAnyObjectByType<CameraFollow>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!followPlayer) cameraFollow.ChangeCameraModeToStatic(false ,newStaticCamPos.position, newStaticCamPos.rotation, camSpeedOnZoneEntering);
            else cameraFollow.ChangeCameraModeToFollowPlayer(false , offsetIfFollowPlayer, newStaticCamPos.rotation, camSpeedOnZoneEntering);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!followPlayer) cameraFollow.ChangeCameraModeToStatic(true ,newStaticCamPos.position, newStaticCamPos.rotation, camSpeedOnZoneExiting);
            else cameraFollow.ChangeCameraModeToFollowPlayer(true , offsetIfFollowPlayer, newStaticCamPos.rotation, camSpeedOnZoneExiting);
        }
    }
}
