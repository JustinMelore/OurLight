using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        FindFirstObjectByType<EndScreen>(FindObjectsInactive.Include).ShowEndingScreen();
    }
}
