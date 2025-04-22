using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the behavior for the main menu
/// </summary>
public class MainMenu : MonoBehaviour
{
    private IntroScreen introScreen;

    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private GameObject creditsButton;
    [SerializeField] private GameObject creditsText;
    [SerializeField] private GameObject backButton;
    
    private void Awake()
    {
        introScreen = FindFirstObjectByType<IntroScreen>(FindObjectsInactive.Include);
        startButton.SetActive(true);
        quitButton.SetActive(true);
        creditsButton.SetActive(true);
        creditsText.SetActive(false);
        backButton.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// Triggers when the start button is clicked. SHows the intro screen
    /// </summary>
    public void OnStart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        EventSystem.current.SetSelectedGameObject(null);
        introScreen.gameObject.SetActive(true);
        introScreen.ShowIntroScreen();
    }

    /// <summary>
    /// Triggers when the credits button is clicked. Shows the game's credits
    /// </summary>
    public void OnCredits()
    {
        startButton.SetActive(false);
        quitButton.SetActive(false);
        creditsButton.SetActive(false);
        creditsText.SetActive(true);
        backButton.SetActive(true);
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    /// <summary>
    /// Triggers when the back button in the credits page is clicked. Returns to the game's regular main menu
    /// </summary>
    public void OnBack()
    {
        startButton.SetActive(true);
        quitButton.SetActive(true);
        creditsButton.SetActive(true);
        creditsText.SetActive(false);
        backButton.SetActive(false);
        EventSystem.current.SetSelectedGameObject(startButton);
    }

    /// <summary>
    /// Triggers when the quit button is clicked. Exits the game
    /// </summary>
    public void OnQuit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
