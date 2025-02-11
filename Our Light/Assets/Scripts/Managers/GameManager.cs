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

    private void Start()
    {
        respawnManager = FindFirstObjectByType<RespawnManager>();
        mainCamera = FindFirstObjectByType<CameraMovement>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerLight = player.gameObject.transform.GetComponentInChildren<PlayerLight>();
    }

    //WILL EVENTUALLY INCLUDE CODE FOR A DEATH SCREEN
    /// <summary>
    /// Handles the player's death
    /// </summary>
    public void KillPlayer()
    {
        player.enabled = false;
        playerLight.enabled = false;
        Respawn();
    }

    /// <summary>
    /// Restarts the current level
    /// </summary>
    public void Respawn()
    {
        player.RevivePlayer();
        playerLight.ResetLight();
        respawnManager.ResetUnsavedLightables();
        player.enabled = true;
        playerLight.enabled = true;
    }

}
