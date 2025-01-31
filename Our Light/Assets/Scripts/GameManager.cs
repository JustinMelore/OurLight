using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles game-wide logic, such as player death and checkpoints
/// </summary>
public class GameManager : MonoBehaviour
{
    //WILL EVENTUALLY INCLUDE CODE FOR A DEATH SCREEN
    /// <summary>
    /// Handles the player's death
    /// </summary>
    public void KillPlayer()
    {
        Debug.Log("YOU DIED");
        Respawn();
    }

    /// <summary>
    /// Restarts the current level
    /// </summary>
    public void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
