using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles actions performed by the player character in response to user input, such as movement.
/// </summary>
public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private CharacterController characterController;
    private GameManager gameManager;
    private Animator animator;

    private float gravity = -9.81f;
    private float moveSpeed = 5f;
    private float deathHeight = -5f;

    private Vector3 playerVelocity;
    private Vector3 inputVector;

    private float buttonPressedTime;
    private float requiredButtonPressTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        gameManager = FindFirstObjectByType<GameManager>();
        animator = GetComponentInChildren<Animator>();
        requiredButtonPressTime = 0.03f;
        buttonPressedTime = 0f;
    }

    /// <summary>
    /// Updates the player character's movement direction based on user input
    /// </summary>
    /// <param name="inputValue">The inputs provided by the user to update the movement off of</param>
    private void OnMove(InputValue inputValue)
    {
        buttonPressedTime = 0f;
        inputVector = inputValue.Get<Vector3>();
        inputVector = Quaternion.Euler(0, -45, 0) * inputVector;
        playerVelocity.x = inputVector.z * moveSpeed;
        playerVelocity.z = -inputVector.x * moveSpeed;
    }

    private void OnDisable()
    {
        animator.SetFloat("Speed", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (characterController.isGrounded && playerVelocity.y < 0) playerVelocity.y = -2f;
        playerVelocity.y += gravity * Time.deltaTime;
        buttonPressedTime += Time.deltaTime;
        if (inputVector != Vector3.zero) animator.SetFloat("Speed", 1);
        else animator.SetFloat("Speed", 0);
        if (buttonPressedTime >= requiredButtonPressTime && inputVector != Vector3.zero)
        {
            characterController.Move(playerVelocity * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(inputVector);
        }
        else
        {
            characterController.Move(new Vector3(0, playerVelocity.y, 0) * Time.deltaTime);
        }
        if (transform.position.y <= deathHeight)
        {
            gameManager.KillPlayer();
        }
    }

    public void RevivePlayer()
    {
        transform.position = FindFirstObjectByType<RespawnManager>().GetRespawnPoint();
    }

    //FOR TESTING PURPOSES, WILL EVENTUALLY BE REMOVED
    private void OnReset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
