using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the behavior for the main menu
/// </summary>
public class MainMenu : MonoBehaviour
{
    private IntroScreen introScreen;
    
    private void Awake()
    {
        introScreen = FindFirstObjectByType<IntroScreen>(FindObjectsInactive.Include);
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
    /// Triggers when the quit button is clicked. Exits the game
    /// </summary>
    public void OnQuit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
