using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private IntroScreen introScreen;
    
    private void Awake()
    {
        introScreen = FindFirstObjectByType<IntroScreen>(FindObjectsInactive.Include);
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnStart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        EventSystem.current.SetSelectedGameObject(null);
        introScreen.gameObject.SetActive(true);
        introScreen.ShowIntroScreen();
    }

    public void OnQuit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
