using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

/// <summary>
/// Handles actions performed by the player character in response to user input, such as movement.
/// </summary>
public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private CharacterController characterController;
    private GameManager gameManager;

    private float gravity = -9.81f;
    private float moveSpeed = 5f;
    private float deathHeight = -5f;

    private Vector3 playerVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        //UNCOMMENT THIS
        //Cursor.lockState = CursorLockMode.Locked;
        gameManager = FindFirstObjectByType<GameManager>();
    }

    /// <summary>
    /// Updates the player character's movement direction based on user input
    /// </summary>
    /// <param name="inputValue">The inputs provided by the user to update the movement off of</param>
    private void OnMove(InputValue inputValue)
    {
        Vector3 inputVector = inputValue.Get<Vector3>();
        playerVelocity.x = inputVector.z * moveSpeed;
        playerVelocity.z = -inputVector.x * moveSpeed;
        if(inputVector != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(inputVector);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (characterController.isGrounded && playerVelocity.y < 0) playerVelocity.y = -2f;
        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
        if (transform.position.y <= deathHeight)
        {
            gameManager.KillPlayer();
        }
    }

    public void RevivePlayer()
    {
        transform.position = FindFirstObjectByType<RespawnManager>().GetRespawnPoint();
    }
}
