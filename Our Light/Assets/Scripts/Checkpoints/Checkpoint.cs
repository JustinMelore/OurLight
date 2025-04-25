using UnityEngine;

/// <summary>
/// Handles behavior for the lampposts that serve as checkpoints throughout the game
/// </summary>
public class Checkpoint : MonoBehaviour
{
    private RespawnManager respawnManager;
    private Vector3 respawnPoint;
    private PlayerLight playerLight;

    private Light checkpointLight;
    private Color startingColor;
    [SerializeField] private Color usedColor;
    private float currentColorChangeTime;
    [SerializeField] private float colorChangeTime;

    private bool used;

    private void Start()
    {
        used = false;
        respawnManager = FindFirstObjectByType<RespawnManager>();
        respawnPoint = transform.GetChild(0).transform.position;
        playerLight = FindFirstObjectByType<PlayerLight>();
        checkpointLight = transform.GetComponentInChildren<Light>();
        startingColor = checkpointLight.color;
    }

    private void Update()
    {
        if (!used || checkpointLight.color == usedColor) return;
        currentColorChangeTime += Time.deltaTime;
        Color currentColor = new Color();
        currentColor.r = Mathf.Lerp(startingColor.r, usedColor.r, currentColorChangeTime / colorChangeTime);
        currentColor.g = Mathf.Lerp(startingColor.g, usedColor.g, currentColorChangeTime / colorChangeTime);
        currentColor.b = Mathf.Lerp(startingColor.b, usedColor.b, currentColorChangeTime / colorChangeTime);
        checkpointLight.color = currentColor;
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (used || other.gameObject.layer != 6) return;
        respawnManager.SetRespawnPoint(respawnPoint);
        respawnManager.ClearUnsavedLightables();
        playerLight.ResetLight();
        used = true;
    }

}
