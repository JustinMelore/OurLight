using UnityEngine;

/// <summary>
/// Handles behavior for the trigger that makes the end screen appear
/// </summary>
public class EndTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Cursor.lockState = CursorLockMode.Locked;
        FindFirstObjectByType<EndScreen>(FindObjectsInactive.Include).ShowEndingScreen();
    }
}
