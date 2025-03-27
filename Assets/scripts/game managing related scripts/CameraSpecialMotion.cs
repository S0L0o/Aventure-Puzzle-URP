using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpecialMotion : MonoBehaviour
{
    private Camera cam;
    public PlayerController playerController;
    public float zoomOnBulletTime = 25f;
    public float bulletTimeZoomSpeed = 1.5f;
    public float exitBulletTimeZoomDuration = .5f;

    private float basicFieldOfView;
    private float maxZoomReached;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        basicFieldOfView = cam.fieldOfView;
    }

    public void EnterBulletTime()
    {
        StartCoroutine(WaitForExitBulletTime());
    }

    public IEnumerator WaitForExitBulletTime()
    {
        while (playerController.isInBulletTime)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoomOnBulletTime, Time.deltaTime * bulletTimeZoomSpeed);
            maxZoomReached = cam.fieldOfView + exitBulletTimeZoomDuration;
            yield return null;
        }

        maxZoomReached = cam.fieldOfView;
        StartCoroutine(ExitBulletTime());
    }

    public IEnumerator ExitBulletTime()
    {
        float elapsedTime = 0f;
        while (elapsedTime < exitBulletTimeZoomDuration)
        {
            cam.fieldOfView = Mathf.Lerp(maxZoomReached, basicFieldOfView, elapsedTime / exitBulletTimeZoomDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cam.fieldOfView = basicFieldOfView;
    }
}
