using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Handles movement for the player's camera
/// </summary>
public class CameraMovement : MonoBehaviour
{

    private Transform player;
    private Camera playerCamera;

    private float originalZoom;
    private float currentZoom;
    private float finalZoom;
    private float zoomDuration;
    private float currentCameraZoomTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = GetComponentInChildren<Camera>();
        originalZoom = playerCamera.orthographicSize;
        currentZoom = originalZoom;
        finalZoom = currentZoom;
        currentCameraZoomTime = 0f;
    }

    void Update()
    {
        if(player.position.y > -2) transform.position = player.position;
        if (currentZoom != finalZoom) ZoomCamera();
    }

    public void StartCameraZoom(float zoomPercentage, float zoomDuration)
    {
        finalZoom = originalZoom * zoomPercentage;
        this.zoomDuration = zoomDuration;
        currentZoom = playerCamera.orthographicSize;
        currentCameraZoomTime = 0f;
    }

    private void ZoomCamera()
    {
        if (playerCamera.orthographicSize == finalZoom)
        {
            currentZoom = finalZoom;
            currentCameraZoomTime = 0f;
            return;
        }
        currentCameraZoomTime += Time.deltaTime;
        playerCamera.orthographicSize = Mathf.Lerp(currentZoom, finalZoom, currentCameraZoomTime / zoomDuration);
    }
}
