using UnityEngine;

/// <summary>
/// Handles movement for the player's camera
/// </summary>
public class CameraMovement : MonoBehaviour
{

    private Transform player;

    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        transform.position = player.position;
    }
}
