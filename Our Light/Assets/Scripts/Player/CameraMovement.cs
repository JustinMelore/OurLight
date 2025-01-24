using UnityEngine;

/// <summary>
/// Handles movement for the player's camera
/// </summary>
public class CameraMovement : MonoBehaviour
{

    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        transform.position = player.transform.position;
    }
}
