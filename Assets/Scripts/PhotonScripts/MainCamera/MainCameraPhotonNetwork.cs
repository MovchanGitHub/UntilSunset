using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

using CnControls;

public class MainCameraPhotonNetwork : MonoBehaviour
{
    public Transform player;

    public float boundX = 1.5f;
    public float boundY = 0.6f;

    private Camera camera;
    private float targetZoom = 5f;
    private float zoomFactor = 3f;
    [SerializeField] private float zoomLerpSpeed = 10f;

    GameObject joystick;

    private void Awake()
    {
        camera = Camera.main;
        targetZoom = camera.orthographicSize;
        joystick = GameObject.Find("Joystick");
#if UNITY_STANDALONE_WIN
        joystick.SetActive(false);
#endif
    }

    private void LateUpdate()
    {
        CalculateNewPosition();
        HandleZoom();
    }

    private void CalculateNewPosition()
    {
        Vector3 delta = Vector3.zero;
        Vector3 playerPosition = player.position;
        Vector3 cameraPosition = transform.position;

        float deltaX = playerPosition.x - cameraPosition.x;
        if (Abs(deltaX) > boundX)
        {
            if (cameraPosition.x < playerPosition.x)
            {
                delta.x = deltaX - boundX;
            }
            else
            {
                delta.x = deltaX + boundX;
            }
        }

        float deltaY = playerPosition.y - cameraPosition.y;
        if (Abs(deltaY) > boundY)
        {
            if (cameraPosition.y < playerPosition.y)
            {
                delta.y = deltaY - boundY;
            }
            else
            {
                delta.y = deltaY + boundY;
            }
        }

        AdjustDelta(ref delta);

        transform.position = new Vector3(delta.x, delta.y, -10);
    }

    private void AdjustDelta(ref Vector3 delta)
    {
        Vector3 newPosition = transform.position;
        delta.x += newPosition.x;
        delta.y += newPosition.y;
    }

    private void HandleZoom()
    {
        float scrollData;
        scrollData = Input.GetAxis("Mouse ScrollWheel");
        targetZoom -= scrollData * zoomFactor;

#if (UNITY_ANDROID)
        if (Input.touchCount == 2)
        {
            Touch touchFirst = Input.GetTouch(0);
            Touch touchSecond = Input.GetTouch(1);

            Vector2 deltaTouchFirst = touchFirst.position - touchFirst.deltaPosition;
            Vector2 deltaTouchSecond = touchSecond.position - touchSecond.deltaPosition;

            float distDelta = (deltaTouchFirst - deltaTouchSecond).magnitude;
            float currentDist = (touchFirst.position - touchSecond.position).magnitude;

            float dist = distDelta - currentDist;
            targetZoom += dist * zoomFactor / 1500;
            targetZoom = Mathf.Clamp(targetZoom, 3f, 9f);
        }
#endif

        targetZoom = Mathf.Clamp(targetZoom, 3f, 9f);
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetZoom, Time.deltaTime * zoomLerpSpeed);
    }
}
