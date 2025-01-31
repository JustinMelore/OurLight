using UnityEngine;

/// <summary>
/// Handles movement for the player's camera
/// </summary>
public class CameraMovement : MonoBehaviour
{

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if(player.position.y > -2) transform.position = player.position;
    }
}
