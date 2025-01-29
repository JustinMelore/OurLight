using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles game-wide logic, such as player death and checkpoints
/// </summary>
public class GameManager : MonoBehaviour
{

    /// <summary>
    /// Restarts the current level
    /// </summary>
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
