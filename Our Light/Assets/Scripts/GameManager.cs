using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles game-wide logic, such as player death and checkpoints
/// </summary>
public class GameManager : MonoBehaviour
{
    private RespawnManager respawnManager;
    private CameraMovement mainCamera;
    private GameObject player;

    private void Start()
    {
        respawnManager = FindFirstObjectByType<RespawnManager>();
        mainCamera = FindFirstObjectByType<CameraMovement>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    //WILL EVENTUALLY INCLUDE CODE FOR A DEATH SCREEN
    /// <summary>
    /// Handles the player's death
    /// </summary>
    public void KillPlayer()
    {
        Debug.Log("YOU DIED");
        player.GetComponent<PlayerController>().enabled = false;
        Respawn();
    }

    /// <summary>
    /// Restarts the current level
    /// </summary>
    public void Respawn()
    {
        Debug.Log(respawnManager.GetRespawnPoint());
        player.GetComponent<PlayerController>().RevivePlayer();
        //player.GetComponent<PlayerController>().enabled = true;
        
    }

}
