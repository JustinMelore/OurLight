using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Handles general behavior for "lightable" objects (objects that can be revealed via the player's light)
/// </summary>
public class Lightable : MonoBehaviour
{
    protected bool isLighted;
    protected float currentLightedTime;
    [SerializeField] protected float requiredTime;
    [SerializeField] protected int lightCost;

    protected float currentCameraZoomOutTime;
    [SerializeField] protected float cameraZoomOutDuration;
    [SerializeField] protected float zoomPercentage;
    protected float currentZoom;
    protected float finalZoom;
    protected Camera playerCamera;
    protected float originalZoom;

    protected List<Collider> lightDetectors;
    protected List<GameObject> revealable;
    protected Collider lightCollider;

    protected PlayerLight playerLight;
    protected RespawnManager respawnManager;

    void Start()
    {
        isLighted = false;
        currentLightedTime = 0f;

        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        originalZoom = playerCamera.orthographicSize;
        currentZoom = originalZoom;
        finalZoom = zoomPercentage * originalZoom;
        currentCameraZoomOutTime = 0f;
        lightDetectors = new List<Collider>();
        revealable = new List<GameObject>();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Transform child = gameObject.transform.GetChild(i);
            if (child.tag == "LightDetector") lightDetectors.Add(child.GetComponent<Collider>());
            else if (child.tag == "Revealable") revealable.Add(child.GameObject());
        }
        playerLight = FindFirstObjectByType<PlayerLight>();
        respawnManager = FindFirstObjectByType<RespawnManager>();
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 9) return;
        currentLightedTime = 0f;
        isLighted = true;
        lightCollider = other;
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 9) return;
        isLighted = false;
        currentZoom = playerCamera.orthographicSize;
    }

    void Update()
    {
        if(lightCollider != null && !lightCollider.enabled)
        {
            OnTriggerExit(lightCollider);
            lightCollider = null;
        }
        if (isLighted)
        {
            currentLightedTime += Time.deltaTime;
            playerCamera.orthographicSize = Mathf.Lerp(originalZoom, finalZoom, currentLightedTime / requiredTime);
            if (currentLightedTime >= requiredTime)
            {
                ChangeLightableState(true);
                currentZoom = finalZoom;
                playerLight.AddLightStack(lightCost);
            }
        }
        else if(currentZoom != originalZoom)
        {
            currentCameraZoomOutTime += Time.deltaTime;
            zoomCameraOut();
        }
    }


    public virtual void ChangeLightableState(bool isRevealed)
    {
        if (isRevealed) respawnManager.AddUnsavedLightable(this);
        foreach (Collider lightDetector in lightDetectors)
            lightDetector.enabled = !isRevealed;
        foreach (GameObject revealed in revealable)
        {
            revealed.GetComponent<Collider>().enabled = isRevealed;
            revealed.GetComponent<MeshRenderer>().enabled = isRevealed;
        }
        isLighted = false;
    }

    /// <summary>
    /// Returns the player's camera to its original orthographic size
    /// </summary>
    protected void zoomCameraOut()
    {
        
        if(playerCamera.orthographicSize >= originalZoom)
        {
            currentZoom = originalZoom;
            currentCameraZoomOutTime = 0f;
            return;
        }
        playerCamera.orthographicSize = Mathf.Lerp(currentZoom, finalZoom / zoomPercentage, currentCameraZoomOutTime / cameraZoomOutDuration);
    }
}
