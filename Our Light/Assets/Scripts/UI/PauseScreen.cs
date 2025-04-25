using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the behavior for the player's pause screen
/// </summary>

public class PauseScreen : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] GameObject resumeButton;
    [SerializeField] GameObject cancelQuitButton;
    [SerializeField] GameObject normalPauseScreen;
    [SerializeField] GameObject confirmQuitScreen;
    private PlayerController player;
    private PlayerLight playerLight;

    private void Awake()
    {
        canvasGroup.alpha = 0f;
        normalPauseScreen.SetActive(true);
        confirmQuitScreen.SetActive(false);
        gameObject.SetActive(false);
        player = FindFirstObjectByType<PlayerController>();
        playerLight = FindFirstObjectByType<PlayerLight>();
    }

    /// <summary>
    /// Reveals the pause screen
    /// </summary>
    public void ShowPauseScreen()
    {
        player.enabled = false;
        playerLight.enabled = false;
        gameObject.SetActive(true);
        canvasGroup.alpha = 1f;
        Cursor.lockState = CursorLockMode.None;
        TogglePauseScreen();
    }

    /// <summary>
    /// Hides the pause screen
    /// </summary>
    public void HidePauseScreen()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Cursor.lockState = CursorLockMode.Locked;
        player.enabled = true;
        playerLight.enabled = true;
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Toggles the pause screen instead of the quit confirmation screen
    /// </summary>
    public void TogglePauseScreen()
    {
        normalPauseScreen.SetActive(true);
        confirmQuitScreen.SetActive(false);
        EventSystem.current.SetSelectedGameObject(resumeButton);
    }

    /// <summary>
    /// Toggles the quit confirmation screen instead of the normal pause screen
    /// </summary>
    public void ToggleQuitScreen()
    {
        normalPauseScreen.SetActive(false);
        confirmQuitScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(cancelQuitButton);
    }
    
    /// <summary>
    /// Triggers when the player decides to quit the game
    /// </summary>
    public void OnConfirmQuit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
