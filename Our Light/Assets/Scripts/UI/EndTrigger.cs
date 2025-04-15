using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Cursor.lockState = CursorLockMode.Locked;
        FindFirstObjectByType<EndScreen>(FindObjectsInactive.Include).ShowEndingScreen();
    }
}
