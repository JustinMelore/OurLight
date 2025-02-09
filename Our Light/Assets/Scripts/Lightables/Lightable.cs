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

    [SerializeField] protected float cameraZoomOutDuration;
    [SerializeField] protected float zoomPercentage;
    [SerializeField] protected List<LightMode> specialModes;
    private CameraMovement playerCamera;

    protected List<Collider> lightDetectors;
    protected List<GameObject> revealable;
    protected Collider lightCollider;

    protected PlayerLight playerLight;
    protected RespawnManager respawnManager;

    void Start()
    {
        isLighted = false;
        currentLightedTime = 0f;
        playerCamera = FindFirstObjectByType<CameraMovement>();
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
        LightMode playerMode = playerLight.GetCurrentMode();
        if (other.gameObject.layer != 9 || (playerMode != LightMode.DEFAULT && !specialModes.Contains(playerMode))) return;
        currentLightedTime = 0f;
        isLighted = true;
        lightCollider = other;
        playerCamera.StartCameraZoom(zoomPercentage, requiredTime);
    }

    protected void OnTriggerExit(Collider other)
    {
        isLighted = false;
        playerCamera.StartCameraZoom(1f, cameraZoomOutDuration);
    }

    void Update()
    {
        LightMode playerMode = playerLight.GetCurrentMode();
        if((lightCollider != null && !lightCollider.enabled) || (playerMode != LightMode.DEFAULT && !specialModes.Contains(playerMode)))
        {
            OnTriggerExit(lightCollider);
            lightCollider = null;
        }
        if (isLighted)
        {
            currentLightedTime += Time.deltaTime;
            if (currentLightedTime >= requiredTime)
            {
                ChangeLightableState(true);
                if(playerMode == LightMode.DEFAULT) playerLight.AddLightStack(lightCost);
                playerCamera.StartCameraZoom(1f, cameraZoomOutDuration);
            }
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
}
