using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles game-wide logic, such as player death and checkpoints
/// </summary>
public class GameManager : MonoBehaviour
{
    private RespawnManager respawnManager;
    private CameraMovement mainCamera;
    private PlayerController player;
    private PlayerLight playerLight;
    private DeathScreen deathScreen;

    private void Awake()
    {
        respawnManager = FindFirstObjectByType<RespawnManager>();
        mainCamera = FindFirstObjectByType<CameraMovement>();
        deathScreen = FindFirstObjectByType<DeathScreen>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerLight = player.gameObject.transform.GetComponentInChildren<PlayerLight>();
    }

    /// <summary>
    /// Handles the player's death
    /// </summary>
    public void KillPlayer()
    {
        player.enabled = false;
        playerLight.enabled = false;
        deathScreen.gameObject.SetActive(true);
        deathScreen.ShowDeathScreen();
    }

    /// <summary>
    /// Restarts the current level
    /// </summary>
    public void Respawn()
    {
        player.RevivePlayer();
        playerLight.ResetLight();
        respawnManager.ResetUnsavedLightables();
        deathScreen.HideDeathScreen();
        player.enabled = true;
        playerLight.enabled = true;
    }

}
