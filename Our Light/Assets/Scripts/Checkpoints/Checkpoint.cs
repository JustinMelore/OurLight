using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private RespawnManager respawnManager;
    private Vector3 respawnPoint;
    private PlayerLight playerLight;

    private bool used;

    private void Start()
    {
        used = false;
        respawnManager = FindFirstObjectByType<RespawnManager>();
        respawnPoint = transform.GetChild(0).transform.position;
        playerLight = FindFirstObjectByType<PlayerLight>();
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if(!used)
        {
            Debug.Log("Checkpoint reached");
            respawnManager.SetRespawnPoint(respawnPoint);
            respawnManager.ClearUnsavedLightables();
            playerLight.ResetLight();
            used = true;
        }
    }

}
