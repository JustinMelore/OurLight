using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lightable : MonoBehaviour
{
    protected bool isLighted;
    protected float currentLightedTime;
    [SerializeField] protected float requiredTime;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
    }

    protected void OnTriggerEnter(Collider other)
    {
        currentLightedTime = 0f;
        isLighted = true;
        lightCollider = other;
    }

    protected void OnTriggerExit(Collider other)
    {
        isLighted = false;
        currentZoom = playerCamera.orthographicSize;
    }

    // Update is called once per frame
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
                foreach (Collider lightDetector in lightDetectors)
                    lightDetector.enabled = false;
                foreach (GameObject revealed in revealable)
                {
                    revealed.GetComponent<Collider>().enabled = true;
                    revealed.GetComponent<MeshRenderer>().enabled = true;
                }
                isLighted = false;
                currentZoom = finalZoom;
            }
        }
        else if(currentZoom != originalZoom)
        {
            currentCameraZoomOutTime += Time.deltaTime;
            zoomCameraOut();
        }
    }

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
